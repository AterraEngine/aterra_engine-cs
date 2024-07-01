// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using System.Text.RegularExpressions;

namespace AterraCore.Common.Types.Nexities;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public readonly partial struct AssetId : IEqualityOperators<AssetId, AssetId, bool>, IEquatable<AssetId> {
    private static readonly Regex Regex = MyRegex();

    public string? PluginNameSpace { get; init; }
    public IEnumerable<string> InternalName { get ; init;}

    // -----------------------------------------------------------------------------------------------------------------
    // Constructors
    // -----------------------------------------------------------------------------------------------------------------
    public AssetId(string? pluginName, IEnumerable<string> nameSpace) {
        PluginNameSpace = pluginName;
        InternalName = nameSpace;
    }

    public AssetId(string assetId) {
        Match match = Regex.Match(assetId);
        PluginNameSpace = match.Groups[1].Success ? match.Groups[1].Value : throw new ArgumentException("Plugin Name could not be determined ");
        InternalName = match.Groups[2].Success ? match.Groups[2].Value.Split('/') : throw new ArgumentException("Namespace for the asset could not be determined");
    }

    // -----------------------------------------------------------------------------------------------------------------
    // Helper Methods
    // -----------------------------------------------------------------------------------------------------------------
    [GeneratedRegex(@"^([a-z0-9_-]+):([a-z0-9\/_-]*[^\/_\-])$", RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.IgnoreCase)]
    private static partial Regex MyRegex();
    
    public static bool TryCreateNew(string assetId, [NotNullWhen(true)] out AssetId? output) {
        Match match = Regex.Match(assetId);
        if (!match.Groups[1].Success || !match.Groups[2].Success) {
            output = null;
            return false;
        }

        output = new AssetId(match.Groups[1].Value, match.Groups[2].Value.Split('/'));
        return true;
    }

    public override string ToString() => $"{PluginNameSpace}:{string.Join('/', InternalName)} | PluginNameSpace:{PluginNameSpace}, InternalName:[{string.Join(", ", InternalName)}]";

    // -----------------------------------------------------------------------------------------------------------------
    // Comparison Methods
    // -----------------------------------------------------------------------------------------------------------------
    public static bool operator ==(AssetId left, AssetId right) => left.Equals(right);
    public static bool operator !=(AssetId left, AssetId right) => !left.Equals(right); 
    
    public override bool Equals(object? obj) => obj is AssetId other && Equals(other);
    public bool Equals(AssetId other) => 
        PluginNameSpace.Equals(other.PluginNameSpace, StringComparison.InvariantCultureIgnoreCase)
        && InternalName.Select(a => a.ToLowerInvariant()).SequenceEqual(other.InternalName.Select(a => a.ToLowerInvariant()));
    
    public override int GetHashCode() => HashCode.Combine(PluginNameSpace, InternalName);
}
