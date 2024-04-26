// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

using System.Reflection;
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
    private Assembly? _currentAssembly;

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public EngineLoader InjectCurrentAssemblyAsPlugin() {
        _currentAssembly = Assembly.GetEntryAssembly();
        return this;
    }
    
    private EngineConfigDto GetEngineConfig() {
        var engineConfigParser = new EngineConfigParser<EngineConfigDto>(_startupLogger);
        if (!engineConfigParser.TryDeserializeFromFile(Paths.ConfigEngine, out EngineConfigDto? configDto)) {
            _startupLogger.Error("Engine config file could not be parsed");
            return new EngineConfigDto().PopulateAsEmpty();
        }
        
        _startupLogger.Information("Engine config file parsed");
        return configDto;
    }
    
    public IEngine Start() {
        _startupLogger.Information("--- Starting Startup sequence ---");
        
        EngineConfigDto configDto = GetEngineConfig();
        configDto.OutputToLog(_startupLogger);
        
        var engineServiceBuilder = new EngineServiceBuilder(_startupLogger);
        
        // Services which may be overriden
        engineServiceBuilder.AssignDefaultServices([
            sc => sc.AddSingleton(EngineLogger.CreateLogger()),
        ]);
        
        _startupLogger.Information("Assigned Default services");
        
        string[] filePaths = configDto.PluginData.Plugins
        .Select(p => Path.Join(configDto.PluginData.RootFolder, p.FilePath))
        .ToArray();

        // string[] filePaths = Directory.GetFiles(Paths.Plugins.Folder);
        
        _startupLogger.Information("All plugin file paths: {paths}", filePaths);
        
        // Load plugins
        var pluginLoader = new PluginLoader(configDto.GameVersion, _startupLogger);
        if (_currentAssembly != null) {
            pluginLoader.InjectAssemblyAsPlugin(_currentAssembly);
            _startupLogger.Information("Current Assembly is inserted as Plugin");
        }
        
        if(!pluginLoader.TryParseAllPlugins(filePaths)){
            _startupLogger.Error("Failed to load plugins. Exiting...");
            Environment.Exit((int)ExitCodes.PluginLoadFail);
        }

        pluginLoader.Plugins
            .SelectMany(p => p.GetServices())
            .ToList()
            .ForEach(s => engineServiceBuilder.ServiceCollection.AddSingleton(s.InstanceType, s.ServiceType));
        
        // After plugins have been loaded
        //      - Finish up with assigning the Static Services (services which may not be overriden)
        //      - Build the actual EngineServices
        engineServiceBuilder.AssignStaticServices([
            sc => sc.AddSingleton<IAssetAtlas, AssetAtlas>(),
            sc => sc.AddSingleton<IEngine, Engine>()
        ]);
        _startupLogger.Information("Assigned Static services");

        engineServiceBuilder.FinishBuilding();
        
        // After this point all plugin data should be assigned
        
        // After this point the actual engine should start churning
        // Warn Quick and dirty for now
        var engine = EngineServices.GetService<IEngine>();
        _startupLogger.Information("Engine instance created");

        
        _startupLogger.Information("--- Startup sequence complete ---");
        return engine;

    }
}