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
using AterraCore.Contracts.DI;
using AterraCore.Contracts.FlexiPlug;
using AterraCore.FlexiPlug;
using AterraCore.Loggers;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using static AterraCore.Extensions.ServiceDescriptorExtension;

namespace AterraEngine;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------

public class EngineConfiguration(ILogger? logger = null) {
    private readonly ILogger _logger = SetLogger(logger);
    private readonly IPluginLoader _pluginLoader = new PluginLoader(SetLogger(logger));
    private readonly IEngineServiceBuilder _engineServiceBuilder = new EngineServiceBuilder(SetLogger(logger));
    private EngineConfigDto? _engineConfigDto;
    public EngineConfigDto EngineConfigDto {
        get => _engineConfigDto ??= new EngineConfigDto().PopulateAsEmpty();
        set => _engineConfigDto = value;
    }
    
    private EngineConfigFlags _engineConfigFlag = EngineConfigFlags.UnConfigured;

    private static ILogger SetLogger(ILogger? logger) => logger ?? StartupLogger.CreateLogger();

    // -----------------------------------------------------------------------------------------------------------------
    // Configuration Methods
    // -----------------------------------------------------------------------------------------------------------------
    public EngineConfiguration ImportAssemblyAsPlugin(Assembly? assembly) {
        // TODO use the pluginLoader to assign the plugin.
        if (assembly is null) {
            _logger.Warning("Assembly could not be assigned as a Plugin");
            return this;
        }
        
        _pluginLoader.InjectAssemblyAsPlugin(assembly);
        
        _logger.Information("Assigned Assembly as a Plugin");
        return this;
    }
    
    public EngineConfiguration AssignDefaultServices() {
        // Services which may be overriden
        _engineServiceBuilder.AssignDefaultServices([
            NewServiceDescriptor<ILogger>(EngineLogger.CreateLogger())
        ]);
        
        _logger.Information("Assigned Default Services correctly");
        _engineConfigFlag |= EngineConfigFlags.AssignedDefaultServices;
        return this;
    }
    
    public EngineConfiguration AssignStaticServices() {
        // TODO make a check though the _engineConfigFlag to see if everything has been setup already
        
        // services which may not be overriden
        _engineServiceBuilder.AssignStaticServices([
            NewServiceDescriptor<IAssetAtlas, AssetAtlas>(ServiceLifetime.Singleton),
            NewServiceDescriptor<IEngine, Engine>(ServiceLifetime.Singleton),
            NewServiceDescriptor<IPluginAtlas, PluginAtlas>(ServiceLifetime.Singleton),
        ]);
        
        _logger.Information("Assigned Static Services correctly");
        _engineConfigFlag |= EngineConfigFlags.AssignedStaticServices;
        return this;
    }
    
    public EngineConfiguration ImportEngineConfig(string filePath, bool outputToLog = true) {
        // TODO make a check though the _engineConfigFlag to see if everything has been setup already
        
        // services which may not be overriden
        var engineConfigParser = new EngineConfigParser<EngineConfigDto>(_logger);
        if (!engineConfigParser.TryDeserializeFromFile(filePath, out EngineConfigDto? configDto)) {
            _logger.Error("Engine config file could not be parsed");
            return this; //the _configDto will be null, so setter of EngineConfigDto will populate as empty
        }

        EngineConfigDto = configDto;
        if (outputToLog) EngineConfigDto.OutputToLog(_logger);

        _logger.Information("Imported Engine Config correctly");
        _engineConfigFlag |= EngineConfigFlags.ImportedEngineConfigDto;
        return this;
    }

    public EngineConfiguration ImportPlugins() {
        // TODO make a check though the _engineConfigFlag to see if everything has been setup already
        
        string[] filePaths = EngineConfigDto.PluginData.Plugins
            .Select(p => Path.Join(EngineConfigDto.PluginData.RootFolder, p.FilePath))
            .ToArray();
        
        _logger.Information("All plugin file paths: {paths}", filePaths);
        
        if(!_pluginLoader.TryParseAllPlugins(filePaths)){
            _engineConfigFlag |= EngineConfigFlags.PluginLoadOrderUnstable;
            _logger.Warning("Failed to load all plugins correctly.");
        }
        else {
            _logger.Information("Plugins successfully loaded.");
        }

        _engineConfigFlag |= EngineConfigFlags.ImportedPlugins;
        return this;
    }

    public EngineConfiguration PluginsAssignServices() {
        // TODO make a check though the _engineConfigFlag to see if everything has been setup already
        
        _pluginLoader.Plugins
            .Select(p => p.GetServices())
            .ToList()
            .ForEach(_engineServiceBuilder.AssignServicesFromPlugins);


        _logger.Information("Assigned Services from Plugins");
        _engineConfigFlag |= EngineConfigFlags.ImportedPluginServices;
        return this;
    }

    public EngineConfiguration AssignDependencyInjectionContainer() {
        // TODO make a check though the _engineConfigFlag to see if everything has been setup already
        if (!_engineConfigFlag.HasFlag(EngineConfigFlags.AssignedDefaultServices 
                                       | EngineConfigFlags.AssignedStaticServices)) {
            _logger.Error("No Default- or Static Engine Services were assigned");
            throw new InvalidOperationException("No Default or Static Engine Services were assigned");
        }

        _engineServiceBuilder.FinishBuilding();
        
        _logger.Information("DI Container built");
        _engineConfigFlag |= EngineConfigFlags.DIContainerBuilt;
        return this;
        
    }

    public IEngine CreateEngine() {
        if (_engineConfigFlag == EngineConfigFlags.UnConfigured) {
            // TODO throw proper error
            throw new Exception();
        }
        
        var engine = EngineServices.GetService<IEngine>();
        _logger.Information("Engine instance created of type: {Type}", engine.GetType().FullName);
        return engine;
    }
}