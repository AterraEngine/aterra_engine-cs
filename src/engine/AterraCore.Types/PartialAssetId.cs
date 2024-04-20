// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Globalization;
using System.Text.RegularExpressions;
namespace AterraCore.Types;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public readonly partial struct PartialAssetId(uint value) : IComparable<PartialAssetId>, IEqualityComparer<PartialAssetId> {
    private static Regex _regex = MyRegex(); 
    
    // PartialAssetId is basically just a fancy ushort
    public uint Value { get; } = value; // which means the max plugin ID will be `FFFFFFFF`

    // -----------------------------------------------------------------------------------------------------------------
    // Constructors
    // -----------------------------------------------------------------------------------------------------------------
    public PartialAssetId() : this(0) {} // Create an empty
    public PartialAssetId(int value) : this(CastToUint(value)) { }
    public PartialAssetId(string value) : this(CastToUint(value)) { }
    
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public override string ToString() => Value.ToString("X").PadLeft(8,'0');
    public string ToStringReadable() {
        string stringValue = ToString();
        return string.Concat(
            stringValue.AsSpan(0, 4), 
            "-", 
            stringValue.AsSpan(4, 4));
    }

    internal static uint CastToUint(string value) {
        if (!_regex.IsMatch(value))
            throw new ArgumentException("Invalid input format.", nameof(value));
        
        return uint.Parse(value, NumberStyles.HexNumber);
    }
    
    private static uint CastToUint(int input) {
        if (input < 0)
            throw new ArgumentOutOfRangeException(nameof(input), "Negative values are not allowed.");
        
        return (uint)input;
    }
    
    [GeneratedRegex("^[0-9a-fA-F]{1,8}$")]
    private static partial Regex MyRegex();
    
    // -----------------------------------------------------------------------------------------------------------------
    // Operators and Comparisons
    // -----------------------------------------------------------------------------------------------------------------
    public static bool operator ==(PartialAssetId left, PartialAssetId right) => left.Equals(right);
    public static bool operator !=(PartialAssetId left, PartialAssetId right) => !(left == right);
    
    public int CompareTo(PartialAssetId other) => Value.CompareTo(other.Value);
    
    public override bool Equals(object? obj) => obj is PartialAssetId other && Equals(other);
    public bool Equals(PartialAssetId other) => Value == other.Value;
    public bool Equals(PartialAssetId x, PartialAssetId y) => x.Value == y.Value;

    public int GetHashCode(PartialAssetId obj) => (int)obj.Value;
    public override int GetHashCode() => (int)Value;
    
}