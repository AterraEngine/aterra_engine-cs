// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

using AterraCore.Common.Data;

namespace Extensions;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public static class ConfigurationWarningsExtensions {
    public static bool IsNominal(this ConfigurationWarnings configurationWarnings) => configurationWarnings == ConfigurationWarnings.Nominal;

    public static bool IsNotNominal(this ConfigurationWarnings configurationWarnings) =>
        !configurationWarnings.IsNominal();
}
