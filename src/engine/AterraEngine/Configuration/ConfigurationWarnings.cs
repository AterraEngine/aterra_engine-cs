// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

namespace AterraEngine.Configuration;

// ---------------------------------------------------------------------------------------------------------------------
// Support Code
// ---------------------------------------------------------------------------------------------------------------------
public static class ConfigurationWarningsExtensions {
    public static bool IsNominal(this ConfigurationWarnings configurationWarnings) => configurationWarnings == ConfigurationWarnings.Nominal;

    public static bool IsNotNominal(this ConfigurationWarnings configurationWarnings) =>
        !configurationWarnings.IsNominal();
}

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[Flags]
public enum ConfigurationWarnings : ulong {
    Nominal = 0ul,

    PluginLoadOrderUnstable = 1ul << 0,
    UnstableAssembly = 1ul << 1,
    UnstablePlugin = 1ul << 2,
    FlowOfOperationsNotRespected = 1ul << 3,
    NoPluginsDefined = 1ul << 4
}