﻿// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

using AterraCore.Contracts;
using AterraCore.Contracts.Nexities.Assets;
using AterraCore.DI;
using AterraCore.Nexities.Assets;
using AterraCore.Common;
using AterraCore.Config.EngineConfig;
using AterraCore.FlexiPlug;
using AterraCore.Loggers;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace AterraEngine;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------

public class EngineLoader {
    private readonly ILogger _startupLogger = StartupLogger.CreateLogger();

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    private EngineConfigDto GetEngineConfig() {
        var engineConfigParser = new EngineConfigParser<EngineConfigDto>(_startupLogger);
        if (!engineConfigParser.TryDeserializeFromFile(Paths.StartupConfig, out EngineConfigDto? configDto)) {
            _startupLogger.Error("Engine config file could not be parsed");
            return new EngineConfigDto().PopulateAsEmpty();
        }
        
        _startupLogger.Information("Engine config file parsed");
        return configDto;
    }
    
    public IEngine Start() {
        EngineConfigDto configDto = GetEngineConfig();
        _startupLogger.Information("Config loaded with the following data:");
        _startupLogger.Information("Engine Version : {Version}", configDto.EngineVersion);
        _startupLogger.Information("Plugins - Root Folder : {Version}", configDto.PluginData.RootFolder);
        _startupLogger.Information("Plugins - Plugins : {Version}", configDto.PluginData.Plugins.Select(r => r.FilePath));
        
        var engineServiceBuilder = new EngineServiceBuilder(_startupLogger);
        
        // Services which may be overriden
        engineServiceBuilder.AssignDefaultServices([
            (sc,_ ) => sc.AddSingleton(EngineLogger.CreateLogger()),
        ]);
        
        _startupLogger.Information("Assigned Default services");
        
        // string[] filePaths = configDto.PluginData.Plugins
        //     .Select(p => Path.Join(configDto.PluginData.RootFolder, p.FilePath))
        //     .ToArray();

        string[] filePaths = Directory.GetFiles(Paths.Plugins.Folder);
        
        _startupLogger.Information("All plugin file paths: {paths}", filePaths);
        
        // Load plugins
        var pluginLoader = new PluginLoader(_startupLogger);
        if(!pluginLoader.TryParseAllPlugins(filePaths)){
            _startupLogger.Error("Failed to load plugins. Exiting...");
            Environment.Exit((int)ExitCodes.PluginLoadFail);
        }
        
        // After plugins have been loaded
        //      - Finish up with assigning the Static Services (services which may not be overriden)
        //      - Build the actual EngineServices
        engineServiceBuilder.AssignStaticServices([
            (sc, _) => sc.AddSingleton<IAssetAtlas, AssetAtlas>(),
            (sc, _) => sc.AddSingleton<IEngine, Engine>()
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