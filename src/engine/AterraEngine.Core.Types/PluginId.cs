// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Globalization;
using System.Text.RegularExpressions;
namespace AterraEngine.Core.Types;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public partial struct PluginId(ushort value) : IComparable<PluginId>, IEqualityComparer<PluginId> {
    private static Regex _regex = MyRegex(); 
    
    // PluginId is basically just a fancy ushort
    public ushort Id { get; } = value; // which means the max plugin ID will be `FFFF`

    // -----------------------------------------------------------------------------------------------------------------
    // Constructors
    // -----------------------------------------------------------------------------------------------------------------
    public PluginId(int value) : this(CastToUshort(value)) { }
    public PluginId(string value) : this(CastToUshort(value)) { }
    
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public override string ToString() {
        return Id.ToString("X").PadLeft(4,'0');
    }

    private static ushort CastToUshort(string value) {
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
    
    [GeneratedRegex(@"^[0-9a-fA-F]{4}$")]
    private static partial Regex MyRegex();
    
    // -----------------------------------------------------------------------------------------------------------------
    // Operators and Comparisons
    // -----------------------------------------------------------------------------------------------------------------
    public static bool operator ==(PluginId left, PluginId right) => left.Equals(right);
    public static bool operator !=(PluginId left, PluginId right) => !(left == right);
    
    public int CompareTo(PluginId other) => Id.CompareTo(other.Id);
    
    public override bool Equals(object? obj) => obj is PluginId other && Equals(other);
    public bool Equals(PluginId other) => Id == other.Id;
    public bool Equals(PluginId x, PluginId y) => x.Id == y.Id;

    public int GetHashCode(PluginId obj) => obj.Id;
    public override int GetHashCode() => Id;
    
}