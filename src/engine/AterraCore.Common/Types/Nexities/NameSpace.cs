﻿// ---------------------------------------------------------------------------------------------------------------------
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
public readonly struct NameSpace :
    IEqualityOperators<NameSpace, NameSpace, bool>,
    IEqualityOperators<NameSpace, string, bool>,
    IEquatable<NameSpace> {
    public IReadOnlyList<string> Values { get; }
    private readonly int _hashCode;
    private readonly ReadOnlyMemory<char> _valueMemory;

    private static readonly ConcurrentDictionary<string, NameSpace> GlobalCache = new();
    // -----------------------------------------------------------------------------------------------------------------
    // Constructors
    // -----------------------------------------------------------------------------------------------------------------
    public NameSpace(string value) {
        if (!GlobalCache.TryGetValue(value, out NameSpace existing)) {
            Match match = RegexLib.Namespaces.Match(value);
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

    public NameSpace(IEnumerable<string> values) {
        string[] valueArray = values.ToArray();
        if (!valueArray.All(value => RegexLib.Namespaces.IsMatch(value)))
            throw new ArgumentException("Invalid Asset Name format.");

        string joined = string.Join('/', valueArray);
        if (GlobalCache.TryGetValue(joined, out NameSpace existing)) {
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
    internal NameSpace(Group matchGroup) {
        Values = matchGroup.Value.Split('/', StringSplitOptions.RemoveEmptyEntries);
        _valueMemory = GetAsMemory(Values);
        _hashCode = ComputeHashCode();
        GlobalCache[matchGroup.Value] = this;
    }

    // -----------------------------------------------------------------------------------------------------------------
    // Implicit Methods
    // -----------------------------------------------------------------------------------------------------------------
    public static implicit operator NameSpace(string s) => new(s);
    public static implicit operator string(NameSpace assetName) => assetName.ToString();

    // -----------------------------------------------------------------------------------------------------------------
    // Helper Methods
    // -----------------------------------------------------------------------------------------------------------------
    public static bool TryCreateNew(string value, [NotNullWhen(true)] out NameSpace? output) {
        Match match = RegexLib.Namespaces.Match(value);
        if (!match.Success) {
            output = null;
            return false;
        }

        output = new NameSpace(match.Groups[1]);
        return true;
    }

    private static ReadOnlyMemory<char> GetAsMemory(IEnumerable<string> values) => new(string.Join('/', values).ToCharArray());

    public override string ToString() => _valueMemory.Span.ToString();

    // -----------------------------------------------------------------------------------------------------------------
    // Comparison Methods
    // -----------------------------------------------------------------------------------------------------------------
    public static bool operator ==(NameSpace left, NameSpace right) => left.Equals(right);

    public static bool operator !=(NameSpace left, NameSpace right) => !left.Equals(right);

    public static bool operator ==(NameSpace left, string? right) =>
        !string.IsNullOrEmpty(right) && TryCreateNew(right, out NameSpace? output) && left.Equals(output);

    public static bool operator !=(NameSpace left, string? right) =>
        string.IsNullOrEmpty(right) || !TryCreateNew(right, out NameSpace? output) || !left.Equals(output);

    public override bool Equals(object? obj) => obj is NameSpace other && Equals(other);

    public bool Equals(NameSpace other) =>
        Values.SequenceEqual(other.Values, StringComparer.OrdinalIgnoreCase);

    public override int GetHashCode() => _hashCode;

    private int ComputeHashCode() => StringComparer.OrdinalIgnoreCase.GetHashCode(_valueMemory);
}
