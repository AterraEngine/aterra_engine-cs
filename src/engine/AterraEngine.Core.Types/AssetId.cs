// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Globalization;
using System.Text.RegularExpressions;
namespace AterraEngine.Core.Types;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public readonly partial struct AssetId : IComparable<AssetId>, IEqualityComparer<AssetId> {
    private static Regex _regex = MyRegex();
    
    public PluginId PluginId { get; }
    public uint Id { get; } // So ... allowing for 4 billion assets per plugin ... that isn't overkill, right?

    // -----------------------------------------------------------------------------------------------------------------
    // Constructors
    // -----------------------------------------------------------------------------------------------------------------
    public AssetId(PluginId pluginId, uint value) {
        PluginId = pluginId;
        Id = value;
    }
    
    public AssetId(uint value) {
        PluginId = new PluginId(0);
        Id = value;
    }
    
    public AssetId(string value) {
        Match match = _regex.Match(value.PadLeft(12,'0'));
        if (!match.Success) {
            throw new ArgumentException("Invalid input format.", nameof(value));
        }
        PluginId = new PluginId(match.Groups[1].Value);
        Id = uint.Parse(match.Groups[2].Value, NumberStyles.HexNumber);
    }

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public override string ToString() {
        // Plugin String already handles hex parsing
        var pluginId = PluginId.ToString();
        string id = Id.ToString("X").PadLeft(8, '0');
        return $"{pluginId}{id}";
    }
    public string ToStringReadable() {
        // Plugin String already handles hex parsing
        var pluginId = PluginId.ToString();
        string id = Id.ToString("X").PadLeft(8, '0');
        return $"{pluginId}-{id}";
    }
    
    [GeneratedRegex("^([0-9a-fA-F]{4})?-?([0-9a-fA-F]{8})$")]
    private static partial Regex MyRegex();
    
    // -----------------------------------------------------------------------------------------------------------------
    // Comparisons
    // -----------------------------------------------------------------------------------------------------------------
    public static bool operator ==(AssetId left, AssetId right) => left.Equals(right);
    public static bool operator !=(AssetId left, AssetId right) => !(left == right);

    public int CompareTo(AssetId other) {
        int pluginIdComparison = PluginId.CompareTo(other.PluginId);
        return (pluginIdComparison != 0)
            ? pluginIdComparison 
            : Id.CompareTo(other.Id);
    }
    
    public override bool Equals(object? obj) =>  obj is AssetId other && Equals(other);
    public bool Equals(AssetId x, AssetId y) => x.PluginId.Equals(y.PluginId) && x.Id == y.Id;
    public bool Equals(AssetId other) => PluginId.Equals(other.PluginId) && Id == other.Id;
    
    public int GetHashCode(AssetId obj) => HashCode.Combine(obj.PluginId, obj.Id);
    public override int GetHashCode() => HashCode.Combine(PluginId, Id);

}