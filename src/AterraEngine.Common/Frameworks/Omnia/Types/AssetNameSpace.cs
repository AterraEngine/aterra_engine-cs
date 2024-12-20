// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using RegexLib=AterraEngine.RegexLib;

// ReSharper disable once CheckNamespace
namespace System;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[StructLayout(LayoutKind.Sequential)]
public readonly struct AssetNameSpace :
    IEquatable<AssetNameSpace>,
    IEqualityOperators<AssetNameSpace, AssetNameSpace, bool>,
    IEqualityOperators<AssetNameSpace, string, bool>,
    IAdditionOperators<AssetNameSpace, AssetPath, OmniaId> {
    public string Value { get; private init; }
    private int HashCode { get; init; }

    private static readonly ConcurrentDictionary<string, AssetNameSpace> Cache = new();

    // -----------------------------------------------------------------------------------------------------------------
    // Constructors
    // -----------------------------------------------------------------------------------------------------------------
    public AssetNameSpace(string value) {
        if (Cache.TryGetValue(value, out AssetNameSpace existing)) {
            Value = existing.Value;
            HashCode = existing.HashCode;
            return;
        }

        Match match = RegexLib.AssetNameSpace.Match(value);
        if (!match.Success) throw new ArgumentException("Invalid Plugin Id format.");

        Value = match.Groups[1].Value;
        HashCode = ComputeHashCode();
        Cache[Value] = this;
    }

    internal AssetNameSpace(Group matchGroup) {
        Value = matchGroup.Value;
        HashCode = ComputeHashCode();
        Cache[Value] = this;
    }

    public static AssetNameSpace Empty => new() {
        Value = string.Empty,
        HashCode = 0
    };

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
    public bool IsEmpty() => HashCode == 0;

    // -----------------------------------------------------------------------------------------------------------------
    // Comparison Methods
    // -----------------------------------------------------------------------------------------------------------------
    public static bool operator ==(AssetNameSpace left, AssetNameSpace right) => left.Equals(right);
    public static bool operator !=(AssetNameSpace left, AssetNameSpace right) => !left.Equals(right);
    public static bool operator ==(AssetNameSpace left, string? right) =>
        !string.IsNullOrEmpty(right) && TryCreateNew(right, out AssetNameSpace? output) && left.Equals(output);

    public static bool operator !=(AssetNameSpace left, string? right) =>
        string.IsNullOrEmpty(right) || !TryCreateNew(right, out AssetNameSpace? output) || !left.Equals(output);

    public static OmniaId operator +(AssetNameSpace left, AssetPath right) => new(left, right);

    public override bool Equals(object? obj) => obj is AssetNameSpace other && Equals(other);

    public bool Equals(AssetNameSpace other) => Value.Equals(other.Value, StringComparison.InvariantCultureIgnoreCase);

    public override int GetHashCode() => HashCode;

    private int ComputeHashCode() => StringComparer.OrdinalIgnoreCase.GetHashCode(Value);
}
