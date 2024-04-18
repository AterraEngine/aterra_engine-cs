// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

namespace AterraCore.Types;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------

public readonly struct PartialAssetId : IComparable<PartialAssetId> {
    public uint Value { get; } // So ... allowing for 4 billion assets per plugin ... that isn't overkill, right?
    
    // -----------------------------------------------------------------------------------------------------------------
    // Constructors
    // -----------------------------------------------------------------------------------------------------------------
    public PartialAssetId(uint value) {
        Value = value;
    }
    public PartialAssetId(int value) {
        if (value < 0) throw new ArgumentOutOfRangeException(nameof(value), "Negative values are not allowed.");
        Value = (uint)value;
    }
    public PartialAssetId(string value) {
        if (!uint.TryParse(value, out uint parsedValue)) throw new ArgumentException("Invalid input format.", nameof(value));
        Value = parsedValue;
    }
    
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public override string ToString() {
        return Value.ToString("X").PadLeft(8, '0');
    }

    
    // -----------------------------------------------------------------------------------------------------------------
    // Comparisons
    // -----------------------------------------------------------------------------------------------------------------
    public static bool operator ==(PartialAssetId left, PartialAssetId right) => left.Equals(right);
    public static bool operator !=(PartialAssetId left, PartialAssetId right) => !left.Equals(right);

    public int CompareTo(PartialAssetId other) {
        return Value.CompareTo(other.Value);
    }

    public override bool Equals(object? obj) =>  obj is PartialAssetId other && Equals(other);
    public bool Equals(PartialAssetId other) => Value == other.Value;

    public override int GetHashCode() {
        return Value.GetHashCode();
    }

    public int GetHashCode(PartialAssetId obj) {
        return (int)obj.Value;
    }
}