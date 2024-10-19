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
public readonly struct AssetPath :
    IEquatable<AssetPath>,
    IEqualityOperators<AssetPath, AssetPath, bool>,
    IEqualityOperators<AssetPath, string, bool>
{
    public IReadOnlyList<string> Values { get; }
    private readonly int _hashCode;
    private readonly ReadOnlyMemory<char> _valueMemory;

    private static readonly ConcurrentDictionary<string, AssetPath> GlobalCache = new();
    // -----------------------------------------------------------------------------------------------------------------
    // Constructors
    // -----------------------------------------------------------------------------------------------------------------
    public AssetPath(string value) {
        if (!GlobalCache.TryGetValue(value, out AssetPath existing)) {
            Match match = RegexLib.AssetPath.Match(value);
            if (!match.Success) throw new ArgumentException("Invalid Asset Name format.");

            Values = match.Groups[1].Value.Split('/', StringSplitOptions.RemoveEmptyEntries);
            _valueMemory = GetAsMemory(Values);
            _hashCode = ComputeHashCode();
            GlobalCache[value] = this;
        }
        else {
            Values = existing.Values;
            _valueMemory = existing._valueMemory;
            _hashCode = existing._hashCode;
        }
    }

    public AssetPath(IEnumerable<string> values) {
        string[] valueArray = values.ToArray();
        if (!valueArray.All(value => RegexLib.AssetPath.IsMatch(value)))
            throw new ArgumentException("Invalid Asset Name format.");

        string joined = string.Join('/', valueArray);
        if (GlobalCache.TryGetValue(joined, out AssetPath existing)) {
            Values = existing.Values;
            _valueMemory = existing._valueMemory;
            _hashCode = existing._hashCode;
        }
        else {
            Values = valueArray;
            _valueMemory = GetAsMemory(valueArray);
            _hashCode = ComputeHashCode();
            GlobalCache[joined] = this;
        }
    }

    // Only supposed to be used by AssetId
    internal AssetPath(Group matchGroup) {
        Values = matchGroup.Value.Split('/', StringSplitOptions.RemoveEmptyEntries);
        _valueMemory = GetAsMemory(Values);
        _hashCode = ComputeHashCode();
        GlobalCache[matchGroup.Value] = this;
    }

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

    public override string ToString() => _valueMemory.Span.ToString();

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

    public override int GetHashCode() => _hashCode;

    private int ComputeHashCode() => StringComparer.OrdinalIgnoreCase.GetHashCode(_valueMemory);
}
