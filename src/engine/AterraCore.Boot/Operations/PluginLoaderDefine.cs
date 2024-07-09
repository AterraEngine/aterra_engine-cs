// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Boot.Logic.PluginLoading;
using AterraCore.Common.Types.Nexities;
using AterraCore.Loggers;
using CodeOfChaos.Extensions;
using static AterraCore.Common.Data.PredefinedAssetIds.NewBootOperationNames;

namespace AterraCore.Boot.Operations;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class PluginLoaderDefine : IBootOperation {
    public AssetId AssetId => PluginLoaderDefineOperation;
    public AssetId? RanAfter => RegisterWarningsOperation;
    private ILogger Logger { get; } = StartupLogger.CreateLogger(false).ForPluginLoaderContext("Define");

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public void Run(IBootOperationComponents components) {
        Logger.Debug("Entered Plugin Loader Creation");
        
        string pluginFolder = components.EngineConfigXml.LoadOrder.RootFolderRelative;
        
        components.PluginLoader.Plugins.AddLastRepeated(
        components.EngineConfigXml.LoadOrder.Plugins.Select(
                dto => new PreLoadedPluginDto(Path.Join(pluginFolder, dto.FilePath))
            )
        );
        
        Logger.Information("Registered PluginLoader with {amount} possible plugins", components.PluginLoader.Plugins.Count);
    }
}
