// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Common.Types.Nexities;
using AterraCore.Loggers;
using static AterraCore.Common.Data.PredefinedAssetIds.NewBootOperationNames;
using static AterraCore.Common.Data.PredefinedAssetIds.NewConfigurationWarnings;

namespace AterraCore.Boot.Operations;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class PluginLoaderPreChecks : IBootOperation {
    public AssetId AssetId => PluginLoaderPreChecksOperation;
    public AssetId? RanAfter => PluginLoaderDefineOperation;
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
