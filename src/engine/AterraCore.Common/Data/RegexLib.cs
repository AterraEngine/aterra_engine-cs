// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Text.RegularExpressions;

namespace AterraCore.Common.Data;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public static partial class RegexLib {
    [GeneratedRegex(@"^([a-z0-9_\-]*[^_\-]):([a-z0-9\/_-]*[^\/_\-])$", RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.IgnoreCase)] 
    private static partial Regex RegexAssetId();
    public static readonly Regex AssetId = RegexAssetId();

    [GeneratedRegex(@"^([a-z0-9_\-]*[^_\-])$", RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.IgnoreCase)]
    private static partial Regex RegexPluginId();
    public static readonly Regex PluginId = RegexPluginId();

    [GeneratedRegex(@"^([a-z0-9\/_\-]*[^\/_\-])$", RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.IgnoreCase)]
    private static partial Regex RegexAssetName();
    public static readonly Regex AssetName = RegexAssetName();
    
    [GeneratedRegex(@"^([a-z0-9_\-]*[^_\-])$", RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.IgnoreCase)]
    private static partial Regex RegexAssetNamePartial();
    public static readonly Regex AssetNamePartial = RegexAssetNamePartial();
}
