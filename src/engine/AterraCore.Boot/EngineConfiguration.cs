// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Boot.FlexiPlug;
using AterraCore.Boot.FlexiPlug.PluginLoading;
using AterraCore.Boot.Nexities;
using AterraCore.Boot.OmniVault;
using AterraCore.Common.ConfigFiles.EngineConfig;
using AterraCore.Common.Data;
using AterraCore.Contracts;
using AterraCore.Contracts.DI;
using AterraCore.Contracts.FlexiPlug;
using AterraCore.DI;
using AterraCore.Loggers;
using AterraEngine;
using Microsoft.Extensions.DependencyInjection;
using static AterraCore.Common.Data.BootFlowOfOperations;
using static AterraCore.Common.Data.ConfigurationWarnings;
using static CodeOfChaos.Extensions.DependencyInjection.ServiceDescriptorExtension;

namespace AterraCore.Boot;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class EngineConfiguration(ILogger? logger = null) : IEngineConfiguration {
    public LinkedList<ServiceDescriptor> ServicesDefault { get; } = [];
    public LinkedList<ServiceDescriptor> ServicesStatic { get; } = [];

    public ConfigurationWarnings Warnings => Nominal;

    public BootFlowOfOperations Flow => UnConfigured;

    public IEngineServiceBuilder EngineServiceBuilder { get; } = new EngineServiceBuilder(GetStartupLogger(logger));
    
    public ILogger StartupLog { get; } = GetStartupLogger(logger);
    public Func<ILogger> EngineLoggerCallback { get; set; } = () => EngineLogger.CreateLogger(false);
    
    private EngineConfigXml? _engineConfig;
    public EngineConfigXml EngineConfig {
        get => _engineConfig ??= new EngineConfigXml();
        set {
            if (_engineConfig is null) {
                _engineConfig = value;
                return;
            }
            StartupLog.Warning("Tried to update Engine Config when it has already been defined");
        }
    }

    private PluginLoader? _pluginLoaderCache;
    private PluginLoader PluginLoader => _pluginLoaderCache ??= new PluginLoader(StartupLog);

    private ISubConfigurations? _subConfigurations;
    public ISubConfigurations SubConfigurations {
        get => _subConfigurations ??= new SubConfigurations(
            new FlexiPlugConfiguration(StartupLog, EngineConfig, PluginLoader),
            new NexitiesConfiguration(StartupLog, EngineConfig, PluginLoader),
            new OmniVaultConfiguration(StartupLog, EngineConfig)
        );
        set {
            if (_subConfigurations is not null) {
                StartupLog.Warning("Tried to update SubConfigurations when it has already been defined");
            } else {
                _subConfigurations = value;
            }
        }
    } 

    // -----------------------------------------------------------------------------------------------------------------
    // Helper Methods
    // -----------------------------------------------------------------------------------------------------------------

    // If the logger is already defined by the program.cs which creates the engine, use that one else use the standard
    private static ILogger GetStartupLogger(ILogger? logger) => logger ?? StartupLogger.CreateLogger(false).ForStartupContext();
    private bool EngineNotPresentAsStaticService() => ServicesStatic.FirstOrDefault(descriptor => descriptor.ServiceType == typeof(IEngine)) != null;

    // -----------------------------------------------------------------------------------------------------------------
    // Configuration Methods
    // -----------------------------------------------------------------------------------------------------------------
    public IEngineConfiguration UseDefaultEngine() => UseCustomEngine<Engine>();
    public IEngineConfiguration UseCustomEngine<T>() where T : IEngine {
        if (EngineNotPresentAsStaticService()) return this;
        ServicesStatic.AddFirst(NewServiceDescriptor<IEngine, T>(ServiceLifetime.Singleton));
        return this;
    }

    public IEngine CreateEngine() {
        // Populate Plugin Atlas with plugin list
        //      Is a singleton anyway, so doesn't matter when we assign this data
        IPluginAtlas pluginAtlas = EngineServices.GetPluginAtlas();
        pluginAtlas.ImportLoadedPluginDtos(PluginLoader.Plugins);

        // Create the Actual Engine
        //  Should be the last step
        IEngine engine = EngineServices.GetEngine();
        StartupLog.Information("Engine instance created of type: {Type}", engine.GetType().FullName);
        return engine;
    }
}
