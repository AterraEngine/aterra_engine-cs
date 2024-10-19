// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Common.Data;
using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

// ReSharper disable once CheckNamespace
namespace System;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[StructLayout(LayoutKind.Sequential)]
public readonly struct AssetNameSpace :
    IEquatable<AssetNameSpace> ,
    IEqualityOperators<AssetNameSpace, AssetNameSpace, bool>,
    IEqualityOperators<AssetNameSpace, string, bool>,
    IAdditionOperators<AssetNameSpace, AssetPath, AssetId>
{
    public string Value { get; }
    private readonly int _hashCode;

    private static readonly ConcurrentDictionary<string, AssetNameSpace> Cache = new();

    // -----------------------------------------------------------------------------------------------------------------
    // Constructors
    // -----------------------------------------------------------------------------------------------------------------
    public AssetNameSpace(string value) {
        if (!Cache.TryGetValue(value, out AssetNameSpace existing)) {
            Match match = RegexLib.AssetNameSpace.Match(value);
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

    internal AssetNameSpace(Group matchGroup) {
        Value = matchGroup.Value;
        _hashCode = ComputeHashCode();
        Cache[Value] = this;
    }

    // -----------------------------------------------------------------------------------------------------------------
    // Implicit Methods
    // -----------------------------------------------------------------------------------------------------------------
    public static implicit operator AssetNameSpace(string s) => new(s);
    public static implicit operator string(AssetNameSpace pluginId) => pluginId.ToString();

    // -----------------------------------------------------------------------------------------------------------------
    // Helper Methods
    // -----------------------------------------------------------------------------------------------------------------
    public static bool TryCreateNew(string value, [NotNullWhen(true)] out AssetNameSpace? output) {
        Match match = RegexLib.AssetNameSpace.Match(value);
        if (!match.Success) {
            output = null;
            return false;
        }

        output = new AssetNameSpace(match.Groups[1]);
        return true;
    }

    public override string ToString() => Value;

    // -----------------------------------------------------------------------------------------------------------------
    // Comparison Methods
    // -----------------------------------------------------------------------------------------------------------------
    public static bool operator ==(AssetNameSpace left, AssetNameSpace right) => left.Equals(right);
    public static bool operator !=(AssetNameSpace left, AssetNameSpace right) => !left.Equals(right);
    public static bool operator ==(AssetNameSpace left, string? right) =>
        !string.IsNullOrEmpty(right) && TryCreateNew(right, out AssetNameSpace? output) && left.Equals(output);

    public static bool operator !=(AssetNameSpace left, string? right) =>
        string.IsNullOrEmpty(right) || !TryCreateNew(right, out AssetNameSpace? output) || !left.Equals(output);

    public static AssetId operator +(AssetNameSpace left, AssetPath right) => new(left, right);

    public override bool Equals(object? obj) => obj is AssetNameSpace other && Equals(other);

    public bool Equals(AssetNameSpace other) => Value.Equals(other.Value, StringComparison.InvariantCultureIgnoreCase);

    public override int GetHashCode() => _hashCode;

    private int ComputeHashCode() => StringComparer.OrdinalIgnoreCase.GetHashCode(Value);
}
