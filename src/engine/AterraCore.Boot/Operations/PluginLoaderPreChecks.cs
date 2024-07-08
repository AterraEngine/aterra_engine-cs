// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Boot.Logic.PluginLoading;
using AterraCore.Common.Types.FlexiPlug;
using AterraCore.Common.Types.Nexities;
using AterraCore.Contracts.Boot.FlexiPlug;
using AterraCore.Loggers;
using CodeOfChaos.Extensions;
using Xml.Elements;
using static AterraCore.Common.Data.PredefinedAssetIds.NewBootOperationNames;
using static AterraCore.Common.Data.PredefinedAssetIds.NewConfigurationWarnings;

namespace AterraCore.Boot.Operations;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class PluginLoaderPreChecks : IBootOperation {
    public AssetId AssetId => PluginLoaderPreChecksOperation;
    public AssetId? RanAfter => PluginLoaderDefineOperation;
    private ILogger Logger { get; } = StartupLogger.CreateLogger(false).ForBootOperationContext("PL : PreChecks");

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public void Run(IBootOperationComponents components) {
        Logger.Debug("Entered Plugin Loader Pre Checks");
        components.PluginLoader = new PluginLoader();

        components.PluginLoader
      
            #region Check FilePath exists
            .IterateOverValid(
                (_, plugin) => {
                    if (File.Exists(plugin.FilePath)) return;
                    plugin.SetInvalid();
                    components.WarningAtlas.RaiseWarningEvent(UnstableFlexiPlugLoadOrder);
                }
            )       
            #endregion

            #region Check Uniqueness
            .IterateOverValid(
                (loader, plugin) => {
                    if (loader.Checksums.Contains(plugin.CheckSum)) {
                        plugin.SetInvalid();
                        components.WarningAtlas.RaiseWarningEvent(DuplicateInPluginLoadOrder);
                    }
                    loader.Checksums.Add(plugin.CheckSum);
                }
            )
            #endregion
        ;
     }
}
