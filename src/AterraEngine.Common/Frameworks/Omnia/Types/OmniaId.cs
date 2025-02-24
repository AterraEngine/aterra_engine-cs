// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;
using RegexLib=AterraEngine.RegexLib;

// ReSharper disable once CheckNamespace
namespace System;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[Serializable]
[StructLayout(LayoutKind.Sequential)]
public struct OmniaId :
    IEquatable<OmniaId>,
    IEqualityOperators<OmniaId, OmniaId, bool>,
    IEqualityOperators<OmniaId, AssetNameSpace, bool>,
    ISerializable {
    private const int MaxLength = 255;
    public AssetNameSpace NameSpace { get; private init; }
    public AssetPath Path { get; private init; }
    [field: NonSerialized] private int HashCode { get; init; }

    #region Caching
    [field: NonSerialized] private ReadOnlyMemory<char> OmniaIdCache { get; init; }

    private record struct CacheObject(AssetNameSpace NameSpace, AssetPath Path, ReadOnlyMemory<char> Cache);

    [NonSerialized] private static readonly ConcurrentDictionary<string, CacheObject> GlobalCache = new();
    #endregion

    #region Empty
    public static OmniaId Empty => new() {
        NameSpace = AssetNameSpace.Empty,
        Path = AssetPath.Empty,
        HashCode = 0,
        OmniaIdCache = ReadOnlyMemory<char>.Empty
    };
    public bool IsEmpty => HashCode == 0;
    #endregion

    // -----------------------------------------------------------------------------------------------------------------
    // Constructors
    // -----------------------------------------------------------------------------------------------------------------
    public OmniaId(string pluginName, IEnumerable<string> nameSpace) : this(new AssetNameSpace(pluginName), new AssetPath(nameSpace as string[] ?? nameSpace.ToArray())) {}

    public OmniaId(string pluginName, string nameSpace) : this(new AssetNameSpace(pluginName), new AssetPath(nameSpace)) {}

    public OmniaId(AssetNameSpace nameSpace, AssetPath path) {
        NameSpace = nameSpace;
        Path = path;
        HashCode = ComputeHashCode();
        OmniaIdCache = GetOrAddCache(nameSpace, path).Cache;

        if (OmniaIdCache.Length <= MaxLength) return;

        GlobalCache.TryRemove(OmniaIdCache.ToString(), out _);
        throw new ArgumentException("OmniaId length cannot exceed 256 characters");
    }

    public OmniaId(string assetId) {
        if (assetId.Length > MaxLength) throw new ArgumentException("OmniaId length cannot exceed 256 characters");

        (AssetNameSpace nameSpace, AssetPath path, ReadOnlyMemory<char> cache) = GlobalCache.GetOrAdd(assetId,
            valueFactory: id => {
                (AssetNameSpace nameSpace, AssetPath path) = ParseOmniaId(id);
                return new CacheObject(nameSpace, path, GetAsMemory(nameSpace, path));
            });

        NameSpace = nameSpace;
        Path = path;
        HashCode = ComputeHashCode();
        OmniaIdCache = cache;
    }

    #region Serialization
    private OmniaId(SerializationInfo info, StreamingContext context) {
        // Deserialize fields
        NameSpace = (AssetNameSpace)info.GetValue(nameof(NameSpace), typeof(AssetNameSpace))!;
        Path = (AssetPath)info.GetValue(nameof(Path), typeof(AssetPath))!;
        HashCode = ComputeHashCode();

        // Rebuild non-serialized field
        OmniaIdCache = GetAsMemory(NameSpace, Path);
    }

    // GetObjectData for ISerializable
    public void GetObjectData(SerializationInfo info, StreamingContext _) {
        info.AddValue(nameof(NameSpace), NameSpace);
        info.AddValue(nameof(Path), Path);
    }
    #endregion

    // -----------------------------------------------------------------------------------------------------------------
    // Implicit Methods
    // -----------------------------------------------------------------------------------------------------------------
    public static implicit operator OmniaId(string s) => new(s);
    public static implicit operator string(OmniaId assetId) => assetId.ToString();

    // -----------------------------------------------------------------------------------------------------------------
    // Helper Methods
    // -----------------------------------------------------------------------------------------------------------------
    public static bool TryCreateNew(string assetId, [NotNullWhen(true)] out OmniaId? output) {
        output = null;
        Match match = RegexLib.OmniaId.Match(assetId);
        if (!match.Success) return false;

        output = new OmniaId(new AssetNameSpace(match.Groups[1]), new AssetPath(match.Groups[2]));
        return true;
    }

    private static (AssetNameSpace, AssetPath) ParseOmniaId(string id) {
        Match match = RegexLib.OmniaId.Match(id);
        if (!match.Success) throw new ArgumentException("Invalid assetId format");

        return (new AssetNameSpace(match.Groups[1]), new AssetPath(match.Groups[2]));
    }

    private static CacheObject GetOrAddCache(AssetNameSpace nameSpace, AssetPath path) {
        string key = nameSpace.Value + ':' + string.Join('/', path);
        return GlobalCache.GetOrAdd(
            key,
            valueFactory: _ => new CacheObject(nameSpace, path, GetAsMemory(nameSpace, path))
        );
    }

    private static ReadOnlyMemory<char> GetAsMemory(AssetNameSpace nameSpace, AssetPath path) {
        string key = nameSpace.Value + ':' + string.Join('/', path);
        return new ReadOnlyMemory<char>(key.ToArray(), 0, key.Length);
    }

    public override string ToString() => OmniaIdCache.ToString();

    public override int GetHashCode() => HashCode;

    private int ComputeHashCode() => System.HashCode.Combine(NameSpace, Path);

    // -----------------------------------------------------------------------------------------------------------------
    // Comparison Methods
    // -----------------------------------------------------------------------------------------------------------------
    public static bool operator ==(OmniaId left, OmniaId right) => left.Equals(right);

    public static bool operator !=(OmniaId left, OmniaId right) => !left.Equals(right);

    public static bool operator ==(OmniaId left, AssetNameSpace right) => left.NameSpace == right;

    public static bool operator !=(OmniaId left, AssetNameSpace right) => left.NameSpace != right;

    public override bool Equals(object? obj) => obj is OmniaId other && Equals(other);

    public bool Equals(OmniaId other) =>
        NameSpace.Equals(other.NameSpace)
        && Path.Equals(other.Path);
}
