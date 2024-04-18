// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Globalization;
using System.Text.RegularExpressions;
namespace AterraCore.Types;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public readonly partial struct AssetId : IComparable<AssetId>, IEqualityComparer<AssetId> {
    private static readonly Regex Regex = MyRegex();
    
    public PluginId PluginId { get; }
    public PartialAssetId Id { get; } 

    // -----------------------------------------------------------------------------------------------------------------
    // Constructors
    // -----------------------------------------------------------------------------------------------------------------
    public AssetId(PluginId pluginId, PartialAssetId value) {
        PluginId = pluginId;
        Id = value;
    }
    
    public AssetId(string value) {
        Match match = Regex.Match(value.PadLeft(12,'0'));
        if (!match.Success) {
            throw new ArgumentException("Invalid input format.", nameof(value));
        }
        PluginId = new PluginId(match.Groups[1].Value);
        Id = new PartialAssetId(match.Groups[2].Value);
    }

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public override string ToString() {
        return $"{PluginId.ToString()}{Id.ToString()}";
    }
    public string ToStringReadable() {
        return $"{PluginId.ToString()}-{Id.ToString()}";
    }
    
    [GeneratedRegex("^([0-9a-fA-F]{4})?-?([0-9a-fA-F]{8})$")]
    private static partial Regex MyRegex();
    
    // -----------------------------------------------------------------------------------------------------------------
    // Comparisons
    // -----------------------------------------------------------------------------------------------------------------
    public static bool operator ==(AssetId left, AssetId right) => left.Equals(right);
    public static bool operator !=(AssetId left, AssetId right) => !left.Equals(right);

    public int CompareTo(AssetId other) {
        int pluginIdComparison = PluginId.CompareTo(other.PluginId);
        return pluginIdComparison != 0
            ? pluginIdComparison 
            : Id.CompareTo(other.Id);
    }
    
    public override bool Equals(object? obj) =>  obj is AssetId other && Equals(other);
    public bool Equals(AssetId x, AssetId y) => x.PluginId.Equals(y.PluginId) && x.Id == y.Id;
    public bool Equals(AssetId other) => PluginId.Equals(other.PluginId) && Id == other.Id;
    
    public int GetHashCode(AssetId obj) => HashCode.Combine(obj.PluginId, obj.Id);
    public override int GetHashCode() => HashCode.Combine(PluginId, Id);

}