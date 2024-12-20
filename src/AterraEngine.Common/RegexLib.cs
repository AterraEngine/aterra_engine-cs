// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Text.RegularExpressions;

namespace AterraEngine;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public static partial class RegexLib {

    private const RegexOptions DefaultOptions = RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.IgnoreCase;

    [GeneratedRegex(@"^(\d+)\.(\d+)\.(\d+)(?:\-(\w*))?$", DefaultOptions)]
    public static partial Regex SemanticVersion { get; }

    // If this one is changed, change the one in https://github.com/AterraEngine/aterra_engine-cs/blob/restructure-loid/tools/AterraEngine.Analyzers/AterraEngine.Tools.Analyzers/Omnia/InvalidAssetId/InvalidAssetIdAnalyzer.cs
    [GeneratedRegex(@"^([a-z0-9](?:[a-z0-9]|(?<![_\-])[_\-](?![_\-]))*[a-z0-9]):([a-z0-9](?:[a-z0-9]|(?<![\/_\-])[\/_\-](?![_\-]))*[a-z0-9])$", DefaultOptions)]
    public static partial Regex OmniaId { get; }

    [GeneratedRegex(@"^([a-z0-9](?:[a-z0-9]|(?<![_\-])[_\-](?![_\-]))*[a-z0-9])$", DefaultOptions)]
    public static partial Regex AssetNameSpace { get; }

    [GeneratedRegex(@"^([a-z0-9](?:[a-z0-9]|(?<![\/_\-])[\/_\-](?![\/_\-]))*[a-z0-9])$", DefaultOptions)]
    public static partial Regex AssetPath { get; }
}
