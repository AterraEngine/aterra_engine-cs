// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Collections.Concurrent;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

namespace AterraCore.Common.Types.Nexities;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[StructLayout(LayoutKind.Sequential)]
public readonly struct AssetTag : IEqualityOperators<AssetTag, AssetTag, bool>, IEquatable<AssetTag>, IEqualityOperators<AssetTag, PluginId, bool> {
    public readonly PluginId PluginId;
    public readonly NameSpace NameSpace;
    private readonly int _hashCode;
    private readonly ReadOnlyMemory<char> _assetIdCache;
    private static readonly ConcurrentDictionary<string, (PluginId pluginId, NameSpace assetName, ReadOnlyMemory<char> cache)> GlobalCache = new();

    // -----------------------------------------------------------------------------------------------------------------
    // Constructors
    // -----------------------------------------------------------------------------------------------------------------
    public AssetTag(string pluginName, IEnumerable<string> nameSpace) : this(new PluginId(pluginName), new NameSpace(nameSpace)) {}

    public AssetTag(string pluginName, string nameSpace) : this(new PluginId(pluginName), new NameSpace(nameSpace)) {}

    public AssetTag(PluginId pluginId, NameSpace assetName) {
        PluginId = pluginId;
        NameSpace = assetName;
        _hashCode = ComputeHashCode();
        _assetIdCache = GetOrAddCache(pluginId, assetName).cache;
    }

    public AssetTag(string assetId) {
        (PluginId pluginId, NameSpace assetName, ReadOnlyMemory<char> cache) = GlobalCache.GetOrAdd(assetId, valueFactory: id => {
            (PluginId pluginId, NameSpace assetName) = ParseAssetTag(id);
            return (pluginId, assetName, GetAsMemory(pluginId, assetName));
        });
        PluginId = pluginId;
        NameSpace = assetName;
        _hashCode = ComputeHashCode();
        _assetIdCache = cache;
    }

    // -----------------------------------------------------------------------------------------------------------------
    // Implicit Methods
    // -----------------------------------------------------------------------------------------------------------------
    public static implicit operator AssetTag(string s) => new(s);
    public static implicit operator string(AssetTag assetId) => assetId.ToString();

    // -----------------------------------------------------------------------------------------------------------------
    // Helper Methods
    // -----------------------------------------------------------------------------------------------------------------
    public static bool TryCreateNew(string assetId, [NotNullWhen(true)] out AssetTag? output) {
        Match match = RegexLib.AssetTag.Match(assetId);
        if (!match.Success) {
            output = null;
            return false;
        }

        output = new AssetTag(new PluginId(match.Groups[1]), new NameSpace(match.Groups[2]));
        return true;
    }

    private static (PluginId, NameSpace) ParseAssetTag(string id) {
        Match match = RegexLib.AssetTag.Match(id);
        if (!match.Success) throw new ArgumentException("Invalid assetId format");

        return (new PluginId(match.Groups[1]), new NameSpace(match.Groups[2]));
    }

    private static (PluginId, NameSpace, ReadOnlyMemory<char> cache) GetOrAddCache(PluginId pluginId, NameSpace assetName) {
        string key = '#' + pluginId.Value + ':' + string.Join('/', assetName);
        return GlobalCache.GetOrAdd(key, valueFactory: _ => (pluginId, assetName, GetAsMemory(pluginId, assetName)));
    }

    private static ReadOnlyMemory<char> GetAsMemory(PluginId pluginId, NameSpace assetName) {
        string key = '#' + pluginId.Value + ':' + string.Join('/', assetName);
        return new ReadOnlyMemory<char>(key.ToArray(), 0, key.Length);
    }

    public override string ToString() => _assetIdCache.ToString();

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override int GetHashCode() => _hashCode;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private int ComputeHashCode() => HashCode.Combine(PluginId, NameSpace);

    // -----------------------------------------------------------------------------------------------------------------
    // Comparison Methods
    // -----------------------------------------------------------------------------------------------------------------
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator ==(AssetTag left, AssetTag right) => left.Equals(right);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator !=(AssetTag left, AssetTag right) => !left.Equals(right);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator ==(AssetTag left, PluginId right) => left.PluginId == right;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator !=(AssetTag left, PluginId right) => left.PluginId != right;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override bool Equals(object? obj) => obj is AssetTag other && Equals(other);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Equals(AssetTag other) =>
        PluginId.Equals(other.PluginId)
        && NameSpace.Equals(other.NameSpace);
}
