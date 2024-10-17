// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Text.RegularExpressions;

namespace AterraCore.Common.Data;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public static partial class RegexLib {
    public static readonly Regex SemanticVersion = RegexSemanticVersion();
    public static readonly Regex AssetId = RegexAssetId();
    public static readonly Regex AssetTag = RegexAssetTag();
    public static readonly Regex PluginId = RegexPluginId();
    public static readonly Regex Namespaces = RegexNamespaces();

    [GeneratedRegex(@"^(\d+)\.(\d+)\.(\d+)(?:\-(\w*))?$", RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.IgnoreCase)]
    private static partial Regex RegexSemanticVersion();

    [GeneratedRegex(@"^([a-z0-9](?:[a-z0-9]|(?<![_\-])[_\-](?![_\-]))*[a-z0-9]):([a-z0-9](?:[a-z0-9]|(?<![\/_\-])[\/_\-](?![_\-]))*[a-z0-9])$", RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.IgnoreCase)]
    private static partial Regex RegexAssetId();

    [GeneratedRegex(@"^#([a-z0-9](?:[a-z0-9]|(?<![_\-])[_\-](?![_\-]))*[a-z0-9]):([a-z0-9](?:[a-z0-9]|(?<![\/_\-])[\/_\-](?![\/_\-]))*[a-z0-9])$", RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.IgnoreCase)]
    private static partial Regex RegexAssetTag();

    [GeneratedRegex(@"^([a-z0-9](?:[a-z0-9]|(?<![_\-])[_\-](?![_\-]))*[a-z0-9])$", RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.IgnoreCase)]
    private static partial Regex RegexPluginId();

    [GeneratedRegex(@"^([a-z0-9](?:[a-z0-9]|(?<![\/_\-])[\/_\-](?![\/_\-]))*[a-z0-9])$", RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.IgnoreCase)]
    private static partial Regex RegexNamespaces();
}
