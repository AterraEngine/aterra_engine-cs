// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Text.RegularExpressions;

namespace AterraEngine.Common.Data;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public static partial class RegexLib {
    public static readonly Regex SemanticVersion = RegexSemanticVersion();
    public static readonly Regex OmniaId = RegexOmniaId();
    public static readonly Regex AssetTag = RegexAssetTag();
    public static readonly Regex AssetNameSpace = RegexAssetNameSpace();
    public static readonly Regex AssetPath = RegexAssetPath();

    [GeneratedRegex(@"^(\d+)\.(\d+)\.(\d+)(?:\-(\w*))?$", RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.IgnoreCase)]
    private static partial Regex RegexSemanticVersion();

    [GeneratedRegex(@"^([a-z0-9](?:[a-z0-9]|(?<![_\-])[_\-](?![_\-]))*[a-z0-9]):([a-z0-9](?:[a-z0-9]|(?<![\/_\-])[\/_\-](?![_\-]))*[a-z0-9])$", RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.IgnoreCase)]
    private static partial Regex RegexOmniaId();

    [GeneratedRegex(@"^#([a-z0-9](?:[a-z0-9]|(?<![_\-])[_\-](?![_\-]))*[a-z0-9]):([a-z0-9](?:[a-z0-9]|(?<![\/_\-])[\/_\-](?![\/_\-]))*[a-z0-9])$", RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.IgnoreCase)]
    private static partial Regex RegexAssetTag();

    [GeneratedRegex(@"^([a-z0-9](?:[a-z0-9]|(?<![_\-])[_\-](?![_\-]))*[a-z0-9])$", RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.IgnoreCase)]
    private static partial Regex RegexAssetNameSpace();

    [GeneratedRegex(@"^([a-z0-9](?:[a-z0-9]|(?<![\/_\-])[\/_\-](?![\/_\-]))*[a-z0-9])$", RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.IgnoreCase)]
    private static partial Regex RegexAssetPath();
}
