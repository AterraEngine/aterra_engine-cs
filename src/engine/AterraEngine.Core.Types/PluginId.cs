// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Globalization;
namespace AterraEngine.Core.Types;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public struct PluginId : IComparable<PluginId>, IEqualityComparer<PluginId> {
    // PluginId is basically just a fancy ushort
    public ushort Id { get; private set; } // which means the max plugin ID will be `FFFF`

    // -----------------------------------------------------------------------------------------------------------------
    // Constructors
    // -----------------------------------------------------------------------------------------------------------------
    public PluginId(int value) {
        Id = (ushort)value;
    }
    public PluginId(string value) {
        ParseFromString(value);
    }
    
    // -----------------------------------------------------------------------------------------------------------------
    // Constructors
    // -----------------------------------------------------------------------------------------------------------------
    public override string ToString() {
        return Id.ToString("X").PadLeft(4,'0');
    }

    private void ParseFromString(string value) {
        Id = ushort.Parse(value, NumberStyles.HexNumber);
    }
    
    // -----------------------------------------------------------------------------------------------------------------
    // Operators and Comparisons
    // -----------------------------------------------------------------------------------------------------------------
    public static bool operator ==(PluginId left, PluginId right) {
        return left.Equals(right);
    }

    public static bool operator !=(PluginId left, PluginId right) {
        return !(left == right);
    }
    
    public int CompareTo(PluginId other) {
        return Id.CompareTo(other.Id);
    }

    public bool Equals(PluginId x, PluginId y) {
        return x.Id == y.Id;
    }

    public int GetHashCode(PluginId obj) {
        return obj.Id;
    }
    public bool Equals(PluginId other) {
        return Id == other.Id;
    }

    public override bool Equals(object? obj) {
        return obj is PluginId other && Equals(other);
    }

    public override int GetHashCode() {
        return Id;
    }
}