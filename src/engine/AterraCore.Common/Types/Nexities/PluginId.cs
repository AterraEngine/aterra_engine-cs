// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Collections.Concurrent;
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
    IAdditionOperators<PluginId, NameSpace, AssetId>,
    IEquatable<PluginId> {
    public string Value { get; }
    private readonly int _hashCode;

    private static readonly ConcurrentDictionary<string, PluginId> Cache = new();

    // -----------------------------------------------------------------------------------------------------------------
    // Constructors
    // -----------------------------------------------------------------------------------------------------------------
    public PluginId(string value) {
        if (!Cache.TryGetValue(value, out PluginId existing)) {
            Match match = RegexLib.PluginId.Match(value);
            if (!match.Success) throw new ArgumentException("Invalid Plugin Id format.");

            Value = match.Groups[1].Value;
            _hashCode = ComputeHashCode();
            Cache[Value] = this;
        }
        else {
            Value = existing.Value;
            _hashCode = existing._hashCode;
        }
    }

    internal PluginId(Group matchGroup) {
        Value = matchGroup.Value;
        _hashCode = ComputeHashCode();
        Cache[Value] = this;
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
        if (!match.Success) {
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
    public static bool operator ==(PluginId left, PluginId right) => left.Equals(right);
    public static bool operator !=(PluginId left, PluginId right) => !left.Equals(right);
    public static bool operator ==(PluginId left, string? right) =>
        !string.IsNullOrEmpty(right) && TryCreateNew(right, out PluginId? output) && left.Equals(output);

    public static bool operator !=(PluginId left, string? right) =>
        string.IsNullOrEmpty(right) || !TryCreateNew(right, out PluginId? output) || !left.Equals(output);

    public static AssetId operator +(PluginId left, NameSpace right) => new(left, right);

    public override bool Equals(object? obj) => obj is PluginId other && Equals(other);

    public bool Equals(PluginId other) => Value.Equals(other.Value, StringComparison.InvariantCultureIgnoreCase);

    public override int GetHashCode() => _hashCode;

    private int ComputeHashCode() => StringComparer.OrdinalIgnoreCase.GetHashCode(Value);
}
