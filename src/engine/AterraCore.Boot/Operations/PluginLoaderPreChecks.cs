// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Loggers;
using static AterraCore.Common.Data.PredefinedAssetIds.NewConfigurationWarnings;

namespace AterraCore.Boot.Operations;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class PluginLoaderPreChecks : IBootOperation {
    private ILogger Logger { get; } = StartupLogger.CreateLogger(false).ForPluginLoaderContext("PreChecks");

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public void Run(IBootOperationComponents components) {
        Logger.Debug("Entered Plugin Loader Pre Checks");

        components.PluginLoader
      
            #region Check FilePath exists
            .IterateOverValid(
                (_, plugin) => {
                    if (File.Exists(plugin.FilePath)) return;
                    plugin.SetInvalid();
                    components.WarningAtlas.RaiseEvent(UnstableFlexiPlugLoadOrder);
                }
            )       
            #endregion

            #region Check Uniqueness
            .IterateOverValid(
                (loader, plugin) => {
                    if (loader.Checksums.Contains(plugin.CheckSum)) {
                        plugin.SetInvalid();
                        components.WarningAtlas.RaiseEvent(DuplicateInPluginLoadOrder);
                    }
                    loader.Checksums.Add(plugin.CheckSum);
                }
            )
            #endregion
        ;
     }
}
