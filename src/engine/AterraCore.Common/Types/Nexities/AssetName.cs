// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Common.Data;
using CodeOfChaos.Extensions;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using System.Text.RegularExpressions;

namespace AterraCore.Common.Types.Nexities;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public readonly struct AssetName : 
    IEqualityOperators<AssetName, AssetName, bool>,
    IEqualityOperators<AssetName, string, bool>,
    IEquatable<AssetName> 
{
    public IEnumerable<string> Values { get; init; } = [];
    public string Value => string.Join('/', Values);
    
    // -----------------------------------------------------------------------------------------------------------------
    // Constructors
    // -----------------------------------------------------------------------------------------------------------------
    public AssetName(string value) {
        Match match = RegexLib.AssetName.Match(value);
        Values = match.Groups[1].Success
            ? match.Groups[1].Value.Split('.', '/')
            : throw new ArgumentException("Plugin Id could not be determined ")
        ;
    }
    
    public AssetName(IEnumerable<string> values) {
        IEnumerable<string> enumerable = values as string[] ?? values.ToArray();
        Values = enumerable.All(value => RegexLib.AssetNamePartial.Match(value).Success)
            ? enumerable
            : throw new ArgumentException("Asset Name could not be determined")
        ;
    }
    
    // Only supposed to be used by AssetId
    internal AssetName(Group matchGroup) {
        Values = matchGroup.Value.Split('.', '/');
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
        Match match = RegexLib.AssetNamePartial.Match(value);
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
    public static bool operator ==(AssetName left, AssetName right) => left.Equals(right);
    public static bool operator !=(AssetName left, AssetName right) => !left.Equals(right);
    
    public static bool operator ==(AssetName left, string? right) => 
        right.IsNotNullOrEmpty() 
        && TryCreateNew(right!, out AssetName? output) 
        && left.Equals(output)
    ;
    public static bool operator !=(AssetName left, string? right) => 
        !right.IsNotNullOrEmpty() 
        && !TryCreateNew(right!, out AssetName? output) 
        && !left.Equals(output)
    ;
    
    public override bool Equals(object? obj) => obj is AssetName other && Equals(other);
    public bool Equals(AssetName other) => Values
        .Select(a => a.ToLowerInvariant())
        .SequenceEqual(other.Values.Select(a => a.ToLowerInvariant()));

    public override int GetHashCode() => Value.GetHashCode();

}
