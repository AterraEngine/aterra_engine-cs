// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Globalization;
using System.Text.RegularExpressions;
namespace AterraCore.Types;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public readonly partial struct PluginId(ushort value) : IComparable<PluginId>, IEqualityComparer<PluginId> {
    private static Regex _regex = MyRegex(); 
    
    // PluginId is basically just a fancy ushort
    public ushort Value { get; } = value; // which means the max plugin ID will be `FFFF`

    // -----------------------------------------------------------------------------------------------------------------
    // Constructors
    // -----------------------------------------------------------------------------------------------------------------
    public PluginId() : this(0) {} // Create an empty
    public PluginId(int value) : this(CastToUshort(value)) { }
    public PluginId(string value) : this(CastToUshort(value)) { }
    
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public override string ToString() {
        return Value.ToString("X").PadLeft(4,'0');
    }

    internal static ushort CastToUshort(string value) {
        if (!_regex.IsMatch(value)) {
            throw new ArgumentException("Invalid input format.", nameof(value));
        }
        return CastToUshort(int.Parse(value, NumberStyles.HexNumber));
    }
    
    private static ushort CastToUshort(int input) {
        if (input is < ushort.MinValue or > ushort.MaxValue) {
            throw new ArgumentOutOfRangeException(nameof(input), "Value is out of range for ushort.");
        }
        return (ushort)input;
    }
    
    [GeneratedRegex(@"^[0-9a-fA-F]{1,4}$")]
    private static partial Regex MyRegex();
    
    // -----------------------------------------------------------------------------------------------------------------
    // Operators and Comparisons
    // -----------------------------------------------------------------------------------------------------------------
    public static bool operator ==(PluginId left, PluginId right) => left.Equals(right);
    public static bool operator !=(PluginId left, PluginId right) => !(left == right);
    
    public int CompareTo(PluginId other) => Value.CompareTo(other.Value);
    
    public override bool Equals(object? obj) => obj is PluginId other && Equals(other);
    public bool Equals(PluginId other) => Value == other.Value;
    public bool Equals(PluginId x, PluginId y) => x.Value == y.Value;

    public int GetHashCode(PluginId obj) => obj.Value;
    public override int GetHashCode() => Value;
    
}