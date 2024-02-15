// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Contracts;
using AterraEngine.Contracts.Core.PluginFramework;
using AterraEngine.Contracts.Core.Startup.Config;
using AterraEngine.Core.Logging;
using AterraEngine.Core.PluginFramework;
using AterraEngine.Core.ServicesFramework;
using AterraEngine.Core.Startup.EngineConfig;
using AterraEngine.Core.Startup.EngineConfig.Dto;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace AterraEngine.Core.Startup;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class EngineFactory(EngineConfigDto? engineConfigDto, IPlugin? defaultPlugin) {
    private EngineConfigDto EngineConfigDto { get; set; } = engineConfigDto ?? EngineConfigDto.CreateEmpty();
    private static readonly ILogger _startupLogger = StartupLogger.CreateLogger();
    
    // -----------------------------------------------------------------------------------------------------------------
    // Constructors
    // -----------------------------------------------------------------------------------------------------------------
    public static EngineFactory CreateFromConfigFile(string filePath, IPlugin? defaultPlugin = null) {
        var engineConfigFactory = new EngineConfigFactory<EngineConfigDto>(_startupLogger);

        if (engineConfigFactory.TryLoadConfigFile(filePath, out EngineConfigDto? engineConfigDto))
            return new EngineFactory(engineConfigDto, defaultPlugin);

        string e = $"Engine Config could not be loaded from filepath: {filePath}";
        _startupLogger.Fatal(e);
        throw new ApplicationException(e);
    }

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    private static void AssignDefaultServices(IServiceCollection serviceCollection) {
        serviceCollection.AddSingleton(LoggerFactory.CreateLogger()); // ILogger
    }

    private static void AssignOverRulesServices(IServiceCollection serviceCollection) {
        // The Actual Engine
        //      This should not be able to be overriden
        serviceCollection.AddSingleton<IAterraEngine, AterraEngine>(); 
    }
    

    public IAterraEngine CreateEngine() {
        // Create the list of plugins
        //      Load from the config file
        var pluginFactory = new PluginFactory(
            _startupLogger,
            true,
            defaultPlugin is not null ? 1 : 0
        );
        pluginFactory.LoadPluginsFromDLLFilePaths(EngineConfigDto.Plugins.Select(p => p.FilePath));

        // Assign the default plugin, which in most cases is the actual main content of the game.
        var plugins = pluginFactory.Plugins.ToList();
        if (defaultPlugin is not null) plugins.Add(defaultPlugin);
        
        _startupLogger.Information("Loaded {count} plugins", plugins.Count);
        
        // Seed the default services
        //      If a plugin overwrites them, that is okay
        var serviceCollection = new ServiceCollection();
        AssignDefaultServices(serviceCollection);

        var pluginLoader = new PluginLoader(_startupLogger, plugins);
        
        // Assigns services which even if they are overloaded in a plugin, they will still use the default one.
        AssignOverRulesServices(serviceCollection);

        // First load the services from the plugins
        //      Only after the Provider has been built,
        //      we can start loading the plugin data as they depend on services
        pluginLoader.AssignPluginServices(serviceCollection);
        EngineServices.BuildServiceProvider(serviceCollection);
        _startupLogger.Information("Service provider built");

        pluginLoader.LoadPluginData();

        return DefaultServices.GetEngine();
    }
}