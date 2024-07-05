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
public readonly struct PluginId : 
    IEqualityOperators<PluginId, PluginId, bool>,
    IEqualityOperators<PluginId, string, bool>,
    IAdditionOperators<PluginId, AssetName, AssetId>,
    IEquatable<PluginId> 
{
    public string Value { get; init; } = string.Empty;
    
    // -----------------------------------------------------------------------------------------------------------------
    // Constructors
    // -----------------------------------------------------------------------------------------------------------------
    public PluginId(string value) {
        Match match = RegexLib.PluginId.Match(value);
        Value = match.Groups[1].Success
            ? match.Groups[1].Value
            : throw new ArgumentException("Plugin Id could not be determined ")
        ;
    }
    
    // Only supposed to be used by AssetId
    internal PluginId(Group matchGroup) {
        Value = matchGroup.Value;
    }
    
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
    public static bool operator ==(PluginId left, PluginId right) => left.Equals(right);
    public static bool operator !=(PluginId left, PluginId right) => !left.Equals(right);
    
    public static bool operator ==(PluginId left, string? right) => 
        right.IsNotNullOrEmpty() 
        && TryCreateNew(right!, out PluginId? output)
        && left.Equals(output)
    ;

    public static bool operator !=(PluginId left, string? right) => 
        !right.IsNotNullOrEmpty() 
        && !TryCreateNew(right!, out PluginId? output)
        && !left.Equals(output)
    ;

    public static AssetId operator +(PluginId left, AssetName right) => new(left, right);
    
    public override bool Equals(object? obj) => obj is PluginId other && Equals(other);
    public bool Equals(PluginId other) => Value.Equals(other.Value, StringComparison.InvariantCultureIgnoreCase);
    
    public override int GetHashCode() => Value.GetHashCode();
}
