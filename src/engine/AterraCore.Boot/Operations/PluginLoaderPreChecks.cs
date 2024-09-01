// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Contracts.Boot.Operations;
using AterraCore.Loggers;

namespace AterraCore.Boot.Operations;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class PluginLoaderPreChecks : IBootOperation {
    private ILogger Logger { get; } = StartupLogger.CreateLogger(false).ForPluginLoaderContext("PreChecks");

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public void Run(IBootComponents components) {
        Logger.Debug("Entered Plugin Loader Pre Checks");

        components.PluginLoader
            #region Check FilePath exists
            .IterateOverValid(
                (_, rawData, plugin) => {
                    if (File.Exists(rawData.FilePath)) return;
                    plugin.SetInvalid();
                    Logger.Error($"Plugin file \"{rawData.FilePath}\" does not exist");
                }
            )
            #endregion
            #region Check Uniqueness
            .IterateOverValid(
                (loader, rawData, plugin) => {
                    if (!rawData.TryGetChecksum(out string? checksum)) {
                        plugin.SetInvalid();
                        Logger.Warning($"Plugin file \"{rawData.FilePath}\" checksum could not be computed");
                        return;
                    }
                    if (loader.Checksums.Add(checksum)) return;

                    plugin.SetInvalid();
                    Logger.Warning($"Plugin file \"{rawData.FilePath}\" checksum already exists");
                }
            )
            #endregion
            ;
    }
}
