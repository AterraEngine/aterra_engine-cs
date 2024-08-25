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
public readonly struct PluginId :
    IEqualityOperators<PluginId, PluginId, bool>,
    IEqualityOperators<PluginId, string, bool>,
    IAdditionOperators<PluginId, AssetName, AssetId>,
    IEquatable<PluginId> {
    public string Value { get; } = string.Empty;
    private readonly int _hashCode;

    // -----------------------------------------------------------------------------------------------------------------
    // Constructors
    // -----------------------------------------------------------------------------------------------------------------
    public PluginId(string value) {
        Match match = RegexLib.PluginId.Match(value);
        if (!match.Groups[1].Success) throw new ArgumentException("Plugin Id could not be determined ");
        Value = match.Groups[1].Value;
        _hashCode = ComputeHashCode();
    }

    // Only supposed to be used by AssetId
    internal PluginId(Group matchGroup) {
        Value = matchGroup.Value;
        _hashCode = ComputeHashCode();
    }

    // -----------------------------------------------------------------------------------------------------------------
    // Implicit Methods
    // -----------------------------------------------------------------------------------------------------------------
    public static implicit operator PluginId(string s) => new(s);
    public static implicit operator string(PluginId pluginId) => pluginId.ToString();

    // -----------------------------------------------------------------------------------------------------------------
    // Helper Methods
    // -----------------------------------------------------------------------------------------------------------------
    public static bool TryCreateNew(string value, [NotNullWhen(true)] out PluginId? output) {
        Match match = RegexLib.PluginId.Match(value);
        if (!match.Groups[1].Success) {
            output = null;
            return false;
        }

        output = new PluginId(match.Groups[1]);
        return true;
    }

    public override string ToString() => Value;

    // -----------------------------------------------------------------------------------------------------------------
    // Comparison Methods
    // -----------------------------------------------------------------------------------------------------------------
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator ==(PluginId left, PluginId right) => left.Equals(right);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator !=(PluginId left, PluginId right) => !left.Equals(right);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator ==(PluginId left, string? right) =>
        right.IsNotNullOrEmpty()
        && TryCreateNew(right!, out PluginId? output)
        && left.Equals(output);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator !=(PluginId left, string? right) =>
        !right.IsNotNullOrEmpty()
        && !TryCreateNew(right!, out PluginId? output)
        && !left.Equals(output);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static AssetId operator +(PluginId left, AssetName right) => new(left, right);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override bool Equals(object? obj) => obj is PluginId other && Equals(other);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Equals(PluginId other) => Value.Equals(other.Value, StringComparison.InvariantCultureIgnoreCase);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override int GetHashCode() => _hashCode;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private int ComputeHashCode() => StringComparer.OrdinalIgnoreCase.GetHashCode(Value);
}
