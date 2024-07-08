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
public class PluginLoaderDefine : IBootOperation {
    public AssetId AssetId => PluginLoaderDefineOperation;
    public AssetId? RanAfter => RegisterWarningsOperation;
    private ILogger Logger { get; } = StartupLogger.CreateLogger(false).ForBootOperationContext("Define PluginLoader");

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public void Run(IBootOperationComponents components) {
        Logger.Debug("Entered Plugin Loader Creation");
        components.PluginLoader = new PluginLoader();
        
        string pluginFolder = components.EngineConfigXml.LoadOrder.RootFolderRelative;
        
        components.PluginLoader.Plugins.AddLastRepeated(
        components.EngineConfigXml.LoadOrder.Plugins.Select(
                dto => new PreLoadedPluginDto(Path.Join(pluginFolder, dto.FilePath))
            )
        );
        
        Logger.Information("Registered PluginLoader with {amount} possible plugins", components.PluginLoader.Plugins.Count);
    }
}
