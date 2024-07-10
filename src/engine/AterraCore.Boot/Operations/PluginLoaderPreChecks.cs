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
                (_, plugin) => {
                    if (File.Exists(plugin.FilePath)) return;
                    plugin.SetInvalid();
                    Logger.Error($"Plugin file \"{plugin.FilePath}\" does not exist");
                }
            )       
            #endregion

            #region Check Uniqueness
            .IterateOverValid(
                (loader, plugin) => {
                    if (loader.Checksums.Contains(plugin.CheckSum)) {
                        plugin.SetInvalid();
                        Logger.Warning($"Plugin file \"{plugin.FilePath}\" checksum already exists");
                    }
                    loader.Checksums.Add(plugin.CheckSum);
                }
            )
            #endregion
        ;
     }
}
