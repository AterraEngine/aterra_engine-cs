// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Common.Data;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using System.Text.RegularExpressions;

namespace AterraCore.Common.Types.Nexities;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public readonly struct AssetId : IEqualityOperators<AssetId, AssetId, bool>, IEquatable<AssetId>, IEqualityOperators<AssetId, PluginId, bool> {
    public PluginId PluginId { get; init; }
    public AssetName AssetName { get; init; }

    // -----------------------------------------------------------------------------------------------------------------
    // Constructors
    // -----------------------------------------------------------------------------------------------------------------
    public AssetId(string pluginName, IEnumerable<string> nameSpace) {
        PluginId = new PluginId(pluginName);
        AssetName = new AssetName(nameSpace);
    }
    
    public AssetId(string pluginName, string nameSpace) {
        PluginId = new PluginId(pluginName);
        AssetName = new AssetName(nameSpace);
    }
    
    public AssetId(PluginId pluginId, AssetName assetName) {
        PluginId = pluginId;
        AssetName = assetName;
    }

    public AssetId(string assetId) {
        Match match = RegexLib.AssetId.Match(assetId);
        PluginId = new PluginId(match.Groups[1].Success
            ? match.Groups[1]
            : throw new ArgumentException("Plugin Name could not be determined ")
        );
        AssetName = new AssetName(match.Groups[2].Success 
            ? match.Groups[2] 
            : throw new ArgumentException("Namespace for the asset could not be determined")
        );
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

    public override string ToString() => $"{PluginId}:{string.Join('/', AssetName)}";

    // -----------------------------------------------------------------------------------------------------------------
    // Comparison Methods
    // -----------------------------------------------------------------------------------------------------------------
    public static bool operator ==(AssetId left, AssetId right) => left.Equals(right);
    public static bool operator !=(AssetId left, AssetId right) => !left.Equals(right); 
    
    public static bool operator ==(AssetId left, PluginId right) => left.PluginId == right;
    public static bool operator !=(AssetId left, PluginId right) => left.PluginId != right;
    
    public override bool Equals(object? obj) => obj is AssetId other && Equals(other);
    public bool Equals(AssetId other) =>
        PluginId.Equals(other.PluginId)
        && AssetName.Equals(other.AssetName)
    ;

    public override int GetHashCode() => HashCode.Combine(PluginId, AssetName);
}
