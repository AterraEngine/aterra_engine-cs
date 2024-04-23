// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

using AterraCore.Contracts;
using AterraCore.Contracts.Nexities.Assets;
using AterraCore.DI;
using AterraCore.Nexities.Assets;
using AterraCore.Common;
using AterraCore.Config.StartupConfig;
using AterraCore.Config.Xml;
using AterraCore.FlexiPlug;
using AterraCore.Loggers;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace AterraEngine;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------

public class EngineLoader {
    private ILogger _startupLogger = StartupLogger.CreateLogger();

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    private EngineConfigDto GetEngineConfig() {
        var engineConfigFactory = new EngineConfigFactory<EngineConfigDto>(_startupLogger);
        if (!engineConfigFactory.TryLoadConfigFile(Paths.StartupConfig, out EngineConfigDto? configDto)) {
            _startupLogger.Error("Engine config ile could not be parsed");
            return EngineConfigDto.CreateEmptyConfigDto();
        }

        return configDto;
    }
    
    public IEngine Start() {
        EngineConfigDto configDto = GetEngineConfig();
        _startupLogger.Information("Config loaded with the following data:");
        _startupLogger.Information("Engine Version : {Version}", configDto.Version);
        _startupLogger.Information("Plugins - Root Folder : {Version}", configDto.PluginData.RootFolder);
        _startupLogger.Information("Plugins - Plugins : {Version}", configDto.PluginData.Plugins.Select(r => r.FilePath));
        
        var engineServiceBuilder = new EngineServiceBuilder(_startupLogger);
        
        // Services which may be overriden
        engineServiceBuilder.AssignDefaultServices([
            sc => sc.AddSingleton(EngineLogger.CreateLogger()),
        ]);
        
        _startupLogger.Information("Assigned Default services");
        
        string[] filePaths = configDto.PluginData.Plugins
            .Select(p => Path.Join(configDto.PluginData.RootFolder, p.FilePath))
            .ToArray();

        _startupLogger.Information("All plugin file paths: {paths}", filePaths);
        
        // Load plugins
        var pluginLoader = new PluginLoader(_startupLogger);
        if(!pluginLoader.TryParseAllPlugins(filePaths)){
            _startupLogger.Error("Failed to load plugins. Exiting...");
            Environment.Exit(-1);
        };
        // configDto.PluginData.Plugins
        //     .Select(data => new {
        //         Data = data, 
        //         Found = pluginDlls.Contains(data.FilePath),
        //         AssemblyFilePath = pluginDlls
        //     })
            // .Where(data => );
        
        // After plugins have been loaded
        //      - Finish up with assigning the Static Services (services which may not be overriden)
        //      - Build the actual EngineServices
        engineServiceBuilder.AssignStaticServices([
            sc => sc.AddSingleton<IAssetAtlas, AssetAtlas>(),
            sc => sc.AddSingleton<IEngine, Engine>()
        ]);
        _startupLogger.Information("Assigned Static services");

        engineServiceBuilder.FinishBuilding();
        _startupLogger.Information("Dependency Container Built");
        
        // After this point all plugin data should be assigned
        
        // After this point the actual engine should start churning
        // Warn Quick and dirty for now
        return EngineServices.GetService<IEngine>();

    }
}