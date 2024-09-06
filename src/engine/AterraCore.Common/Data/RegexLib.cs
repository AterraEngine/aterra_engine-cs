// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Text.RegularExpressions;

namespace AterraCore.Common.Data;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public static partial class RegexLib {
    [GeneratedRegex(@"^(\d+)\.(\d+)\.(\d+)(?:\-(\w*))?$", RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.IgnoreCase)]
    private static partial Regex RegexSemanticVersion();
    public static readonly Regex SemanticVersion = RegexSemanticVersion();

    // Yes this is just twice the `AssetPartial`, seperated with a `:`
    [GeneratedRegex(@"^([a-z0-9](?:[a-z0-9]|(?<![\/_\-\.\\])[\/_\-\.\\](?![\/_\-\.\\]))*[a-z0-9]):([a-z0-9](?:[a-z0-9]|(?<![\/_\-\.\\])[\/_\-\.\\](?![\/_\-\.\\]))*[a-z0-9])$", RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.IgnoreCase)]
    private static partial Regex RegexAssetId();
    public static readonly Regex AssetId = RegexAssetId();

    [GeneratedRegex(@"^#([a-z0-9](?:[a-z0-9]|(?<![\/_\-\.\\])[\/_\-\.\\](?![\/_\-\.\\]))*[a-z0-9]):([a-z0-9](?:[a-z0-9]|(?<![\/_\-\.\\])[\/_\-\.\\](?![\/_\-\.\\]))*[a-z0-9])$", RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.IgnoreCase)]
    private static partial Regex RegexAssetTag();
    public static readonly Regex AssetTag = RegexAssetTag();

    [GeneratedRegex(@"^([a-z0-9](?:[a-z0-9]|(?<![\/_\-\.\\])[\/_\-\.\\](?![\/_\-\.\\]))*[a-z0-9])$", RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.IgnoreCase)]
    private static partial Regex RegexAssetPartial();
    public static readonly Regex AssetPartial = RegexAssetPartial();
}
