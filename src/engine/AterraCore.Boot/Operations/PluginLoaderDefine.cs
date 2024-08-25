// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Boot.Logic.PluginLoading.Dto;
using AterraCore.Contracts.Boot.Logic.PluginLoading.Dto;
using AterraCore.Contracts.Boot.Operations;
using AterraCore.Loggers;
using CodeOfChaos.Extensions;

namespace AterraCore.Boot.Operations;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class PluginLoaderDefine : IBootOperation {
    private ILogger Logger { get; } = StartupLogger.CreateLogger(false).ForPluginLoaderContext("Define");

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public void Run(IBootComponents components) {
        Logger.Debug("Entered Plugin Loader Creation");
        
        string pluginFolder = components.EngineConfigXml.LoadOrder.RootFolderRelative;
        
        components.PluginLoader.Plugins.AddLastRepeated(
            components.EngineConfigXml.LoadOrder.Plugins.Select(
                dto => ((IRawPluginBootDto rawPluginBootDto, IPluginBootDto pluginBootDto))(
                    new RawPluginBootDto(Path.Join(pluginFolder, dto.FilePath)),
                    new PluginBootDto {FilePath = Path.Join(pluginFolder, dto.FilePath)}
                ))
            );
        
        Logger.Information("Registered FilePathPluginLoader with {amount} possible plugins", components.PluginLoader.Plugins.Count);
    }
}
