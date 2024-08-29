// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Common.Data;
using CodeOfChaos.Extensions;
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
public readonly struct AssetName :
    IEqualityOperators<AssetName, AssetName, bool>,
    IEqualityOperators<AssetName, string, bool>,
    IEquatable<AssetName> {
    public IEnumerable<string> Values { get; } = [];
    public string Value => string.Join('/', Values);
    private readonly int _hashCode;

    // -----------------------------------------------------------------------------------------------------------------
    // Constructors
    // -----------------------------------------------------------------------------------------------------------------
    public AssetName(string value) {
        Match match = RegexLib.AssetPartial.Match(value);
        if (!match.Groups[1].Success) throw new ArgumentException("Plugin Id could not be determined ");
        Values = match.Groups[1].Value.Split('.', '/');
        _hashCode = ComputeHashCode();
    }

    public AssetName(IEnumerable<string> values) {
        IEnumerable<string> enumerable = values as string[] ?? values.ToArray();
        if (!enumerable.All(value => RegexLib.AssetPartial.Match(value).Success)) throw new ArgumentException("Asset Name could not be determined");
        Values = enumerable;
        _hashCode = ComputeHashCode();
    }

    // Only supposed to be used by AssetId
    internal AssetName(Group matchGroup) {
        Values = matchGroup.Value.Split('.', '/');
        _hashCode = ComputeHashCode();
    }

    // -----------------------------------------------------------------------------------------------------------------
    // Implicit Methods
    // -----------------------------------------------------------------------------------------------------------------
    public static implicit operator AssetName(string s) => new(s);
    public static implicit operator string(AssetName assetName) => assetName.ToString();

    // -----------------------------------------------------------------------------------------------------------------
    // Helper Methods
    // -----------------------------------------------------------------------------------------------------------------
    public static bool TryCreateNew(string value, [NotNullWhen(true)] out AssetName? output) {
        Match match = RegexLib.AssetPartial.Match(value);
        if (!match.Groups[1].Success) {
            output = null;
            return false;
        }

        output = new AssetName(match.Groups[1]);
        return true;
    }

    public override string ToString() => Value;

    // -----------------------------------------------------------------------------------------------------------------
    // Comparison Methods
    // -----------------------------------------------------------------------------------------------------------------
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator ==(AssetName left, AssetName right) => left.Equals(right);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator !=(AssetName left, AssetName right) => !left.Equals(right);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator ==(AssetName left, string? right) =>
        right.IsNotNullOrEmpty()
        && TryCreateNew(right!, out AssetName? output)
        && left.Equals(output);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator !=(AssetName left, string? right) =>
        !right.IsNotNullOrEmpty()
        && !TryCreateNew(right!, out AssetName? output)
        && !left.Equals(output);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override bool Equals(object? obj) => obj is AssetName other && Equals(other);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Equals(AssetName other)
        => Values.SequenceEqual(other.Values, StringComparer.OrdinalIgnoreCase);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override int GetHashCode() => _hashCode;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private int ComputeHashCode() {
        return Values.Aggregate(
            17,
            func: (current, value) => current * 31 + StringComparer.OrdinalIgnoreCase.GetHashCode(value)
        );
    }
}
