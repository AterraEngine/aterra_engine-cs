// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Common.Data;
using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

namespace AterraCore.Common.Types.Nexities;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[StructLayout(LayoutKind.Sequential)]
public readonly struct AssetId : IEqualityOperators<AssetId, AssetId, bool>, IEquatable<AssetId>, IEqualityOperators<AssetId, PluginId, bool> {
    public PluginId PluginId { get; }
    public AssetName AssetName { get; }
    private readonly int _hashCode;
    private readonly string _assetIdCache;

    // -----------------------------------------------------------------------------------------------------------------
    // Constructors
    // -----------------------------------------------------------------------------------------------------------------
    public AssetId(string pluginName, IEnumerable<string> nameSpace) {
        PluginId = new PluginId(pluginName);
        AssetName = new AssetName(nameSpace);
        _hashCode = ComputeHashCode();
        _assetIdCache = GetAsString(PluginId, AssetName);
    }
    
    public AssetId(string pluginName, string nameSpace) {
        PluginId = new PluginId(pluginName);
        AssetName = new AssetName(nameSpace);
        _hashCode = ComputeHashCode();
        _assetIdCache = GetAsString(PluginId, AssetName);
    }
    
    public AssetId(PluginId pluginId, AssetName assetName) {
        PluginId = pluginId;
        AssetName = assetName;
        _hashCode = ComputeHashCode();
        _assetIdCache = GetAsString(PluginId, AssetName);
    }

    
    private static readonly ConcurrentDictionary<string, (PluginId, AssetName)> Cache = new();
    private static (PluginId, AssetName) ValueFactory(string id) {
        Match match = RegexLib.AssetId.Match(id);

        Group pluginNameGroup = match.Groups[1];
        Group namespaceGroup = match.Groups[2];

        if (!pluginNameGroup.Success) throw new ArgumentException("Plugin Name could not be determined");
        if (!namespaceGroup.Success) throw new ArgumentException("Namespace for the asset could not be determined");

        return (new PluginId(pluginNameGroup.Value), new AssetName(namespaceGroup.Value));
    }
    public AssetId(string assetId) {
        (PluginId, AssetName) tuple = Cache.GetOrAdd(assetId, ValueFactory);
        PluginId = tuple.Item1;
        AssetName = tuple.Item2;
        _hashCode = ComputeHashCode();
        _assetIdCache = GetAsString(PluginId, AssetName);
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
        Match match = RegexLib.AssetId.Match(assetId);
        if (!match.Groups[1].Success || !match.Groups[2].Success) {
            output = null;
            return false;
        }

        output = new AssetId(new PluginId(match.Groups[1]), new AssetName(match.Groups[2]));
        return true;
    }

    private static string GetAsString(PluginId pluginId, AssetName assetName) => $"{pluginId}:{string.Join('/', assetName)}";
    public override string ToString() => _assetIdCache;
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override int GetHashCode() => _hashCode;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private int ComputeHashCode() {
        int hash = 17;
        hash = hash * 31 + PluginId.GetHashCode();
        hash = hash * 31 + AssetName.GetHashCode();
        return hash;
    }
    
    // -----------------------------------------------------------------------------------------------------------------
    // Comparison Methods
    // -----------------------------------------------------------------------------------------------------------------
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator ==(AssetId left, AssetId right) => left.Equals(right);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator !=(AssetId left, AssetId right) => !left.Equals(right); 
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator ==(AssetId left, PluginId right) => left.PluginId == right;
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator !=(AssetId left, PluginId right) => left.PluginId != right;
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override bool Equals(object? obj) => obj is AssetId other && Equals(other);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Equals(AssetId other) =>
        PluginId.Equals(other.PluginId)
        && AssetName.Equals(other.AssetName)
    ;
}
