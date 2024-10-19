// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Common.Data;
using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

// ReSharper disable once CheckNamespace
namespace System;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[StructLayout(LayoutKind.Sequential)]
public readonly struct AssetId :
    IEquatable<AssetId>,
    IEqualityOperators<AssetId, AssetId, bool>,
    IEqualityOperators<AssetId, AssetNameSpace, bool> {
    private const int MaxLength = 255;
    public readonly AssetNameSpace NameSpace;
    public readonly AssetPath Path;
    private readonly int _hashCode;
    private readonly ReadOnlyMemory<char> _assetIdCache;
    private static readonly ConcurrentDictionary<string, (AssetNameSpace nameSpace, AssetPath path, ReadOnlyMemory<char> cache)> GlobalCache = new();

    // -----------------------------------------------------------------------------------------------------------------
    // Constructors
    // -----------------------------------------------------------------------------------------------------------------
    public AssetId(string pluginName, IEnumerable<string> nameSpace) : this(new AssetNameSpace(pluginName), new AssetPath(nameSpace)) {}

    public AssetId(string pluginName, string nameSpace) : this(new AssetNameSpace(pluginName), new AssetPath(nameSpace)) {}

    public AssetId(AssetNameSpace nameSpace, AssetPath path) {
        NameSpace = nameSpace;
        Path = path;
        _hashCode = ComputeHashCode();
        _assetIdCache = GetOrAddCache(nameSpace, path).cache;

        if (_assetIdCache.Length <= MaxLength) return;

        GlobalCache.TryRemove(_assetIdCache.ToString(), out _);
        throw new ArgumentException("OmniaId length cannot exceed 256 characters");
    }

    public AssetId(string assetId) {
        if (assetId.Length > MaxLength) throw new ArgumentException("OmniaId length cannot exceed 256 characters");

        (AssetNameSpace nameSpace, AssetPath path, ReadOnlyMemory<char> cache) = GlobalCache.GetOrAdd(assetId, valueFactory: id => {
            (AssetNameSpace nameSpace, AssetPath path) = ParseOmniaId(id);
            return (nameSpace, path, GetAsMemory(nameSpace, path));
        });

        NameSpace = nameSpace;
        Path = path;
        _hashCode = ComputeHashCode();
        _assetIdCache = cache;
    }

    // -----------------------------------------------------------------------------------------------------------------
    // Implicit Methods
    // -----------------------------------------------------------------------------------------------------------------
    public static implicit operator AssetId(string s) => new(s);
    public static implicit operator string(AssetId assetId) => assetId.ToString();

    // -----------------------------------------------------------------------------------------------------------------
    // Helper Methods
    // -----------------------------------------------------------------------------------------------------------------
    public static bool TryCreateNew(string assetId, [NotNullWhen(true)] out AssetId? output) {
        output = null;
        Match match = RegexLib.OmniaId.Match(assetId);
        if (!match.Success) return false;

        output = new AssetId(new AssetNameSpace(match.Groups[1]), new AssetPath(match.Groups[2]));
        return true;
    }

    private static (AssetNameSpace, AssetPath) ParseOmniaId(string id) {
        Match match = RegexLib.OmniaId.Match(id);
        if (!match.Success) throw new ArgumentException("Invalid assetId format");

        return (new AssetNameSpace(match.Groups[1]), new AssetPath(match.Groups[2]));
    }

    private static (AssetNameSpace, AssetPath, ReadOnlyMemory<char> cache) GetOrAddCache(AssetNameSpace nameSpace, AssetPath path) {
        string key = nameSpace.Value + ':' + string.Join('/', path);
        return GlobalCache.GetOrAdd(
            key,
            valueFactory: _ => (nameSpace, path, GetAsMemory(nameSpace, path))
        );
    }

    private static ReadOnlyMemory<char> GetAsMemory(AssetNameSpace nameSpace, AssetPath path) {
        string key = nameSpace.Value + ':' + string.Join('/', path);
        return new ReadOnlyMemory<char>(key.ToArray(), 0, key.Length);
    }

    public override string ToString() => _assetIdCache.ToString();

    public override int GetHashCode() => _hashCode;

    private int ComputeHashCode() => HashCode.Combine(NameSpace, Path);

    // -----------------------------------------------------------------------------------------------------------------
    // Comparison Methods
    // -----------------------------------------------------------------------------------------------------------------
    public static bool operator ==(AssetId left, AssetId right) => left.Equals(right);

    public static bool operator !=(AssetId left, AssetId right) => !left.Equals(right);

    public static bool operator ==(AssetId left, AssetNameSpace right) => left.NameSpace == right;

    public static bool operator !=(AssetId left, AssetNameSpace right) => left.NameSpace != right;

    public override bool Equals(object? obj) => obj is AssetId other && Equals(other);

    public bool Equals(AssetId other) =>
        NameSpace.Equals(other.NameSpace)
        && Path.Equals(other.Path);
}
