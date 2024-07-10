// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Boot.Logic.PluginLoading;
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
    public void Run(IBootOperationComponents components) {
        Logger.Debug("Entered Plugin Loader Creation");
        
        string pluginFolder = components.EngineConfigXml.LoadOrder.RootFolderRelative;
        
        components.PluginLoader.Plugins.AddLastRepeated(
        components.EngineConfigXml.LoadOrder.Plugins.Select(
                dto => new FilePathLoadedPluginDto(Path.Join(pluginFolder, dto.FilePath))
            )
        );
        
        Logger.Information("Registered FilePathPluginLoader with {amount} possible plugins", components.PluginLoader.Plugins.Count);
    }
}
