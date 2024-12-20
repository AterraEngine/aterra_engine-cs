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
public readonly struct AssetPath :
    IEquatable<AssetPath>,
    IEqualityOperators<AssetPath, AssetPath, bool>,
    IEqualityOperators<AssetPath, string, bool> {
    public IReadOnlyList<string> Values { get; private init; }
    private int HashCode { get; init; }
    private ReadOnlyMemory<char> ValueMemory { get; init; }

    private static readonly ConcurrentDictionary<string, AssetPath> GlobalCache = new();
    // -----------------------------------------------------------------------------------------------------------------
    // Constructors
    // -----------------------------------------------------------------------------------------------------------------
    public AssetPath(string value) {
        if (!GlobalCache.TryGetValue(value, out AssetPath existing)) {
            Match match = RegexLib.AssetPath.Match(value);
            if (!match.Success) throw new ArgumentException("Invalid Asset Name format.");

            Values = match.Groups[1].Value.Split('/', StringSplitOptions.RemoveEmptyEntries);
            ValueMemory = GetAsMemory(Values);
            HashCode = ComputeHashCode();
            GlobalCache[value] = this;
        }
        else {
            Values = existing.Values;
            ValueMemory = existing.ValueMemory;
            HashCode = existing.HashCode;
        }
    }

    // Only supposed to be used by AssetId
    internal AssetPath(Group matchGroup) {
        Values = matchGroup.Value.Split('/', StringSplitOptions.RemoveEmptyEntries);
        ValueMemory = GetAsMemory(Values);
        HashCode = ComputeHashCode();
        GlobalCache[matchGroup.Value] = this;
    }
    
    public AssetPath(params string[] values) {
        if (!values.All(value => RegexLib.AssetPath.IsMatch(value)))
            throw new ArgumentException("Invalid Asset Name format.");

        string joined = string.Join('/', values);
        if (GlobalCache.TryGetValue(joined, out AssetPath existing)) {
            Values = existing.Values;
            ValueMemory = existing.ValueMemory;
            HashCode = existing.HashCode;
            return;
        }

        Values = values;
        ValueMemory = GetAsMemory(values);
        HashCode = ComputeHashCode();
        GlobalCache[joined] = this;
    }

    public static AssetPath Empty => new() {
        Values = [],
        HashCode = 0,
        ValueMemory = ReadOnlyMemory<char>.Empty
    };

    // -----------------------------------------------------------------------------------------------------------------
    // Implicit Methods
    // -----------------------------------------------------------------------------------------------------------------
    public static implicit operator AssetPath(string s) => new(s);
    public static implicit operator string(AssetPath assetName) => assetName.ToString();

    // -----------------------------------------------------------------------------------------------------------------
    // Helper Methods
    // -----------------------------------------------------------------------------------------------------------------
    public static bool TryCreateNew(string value, [NotNullWhen(true)] out AssetPath? output) {
        Match match = RegexLib.AssetPath.Match(value);
        if (!match.Success) {
            output = null;
            return false;
        }

        output = new AssetPath(match.Groups[1]);
        return true;
    }

    private static ReadOnlyMemory<char> GetAsMemory(IEnumerable<string> values) => new(string.Join('/', values).ToCharArray());

    public override string ToString() => ValueMemory.Span.ToString();

    public bool IsEmpty() => HashCode == 0;

    // -----------------------------------------------------------------------------------------------------------------
    // Comparison Methods
    // -----------------------------------------------------------------------------------------------------------------
    public static bool operator ==(AssetPath left, AssetPath right) => left.Equals(right);

    public static bool operator !=(AssetPath left, AssetPath right) => !left.Equals(right);

    public static bool operator ==(AssetPath left, string? right) =>
        !string.IsNullOrEmpty(right) && TryCreateNew(right, out AssetPath? output) && left.Equals(output);

    public static bool operator !=(AssetPath left, string? right) =>
        string.IsNullOrEmpty(right) || !TryCreateNew(right, out AssetPath? output) || !left.Equals(output);

    public override bool Equals(object? obj) => obj is AssetPath other && Equals(other);

    public bool Equals(AssetPath other) =>
        Values.SequenceEqual(other.Values, StringComparer.OrdinalIgnoreCase);

    public override int GetHashCode() => HashCode;

    private int ComputeHashCode() => StringComparer.OrdinalIgnoreCase.GetHashCode(ValueMemory);
}
