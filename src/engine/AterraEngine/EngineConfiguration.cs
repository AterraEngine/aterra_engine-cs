// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

using System.Reflection;
using AterraCore.Contracts;
using AterraCore.Contracts.Nexities.Assets;
using AterraCore.DI;
using AterraCore.Nexities.Assets;
using AterraCore.Common;
using AterraCore.Common.Config;
using AterraCore.Contracts.FlexiPlug;
using AterraCore.Contracts.FlexiPlug.Plugin;
using AterraCore.Contracts.Renderer;
using AterraCore.FlexiPlug;
using AterraCore.Loggers;
using AterraEngine.Config;
using AterraEngine.Renderer.Raylib;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Extensions;
using Xml;
using static Extensions.ServiceDescriptorExtension;

namespace AterraEngine;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------

public class EngineConfiguration(ILogger? logger = null) {
    private readonly ILogger _logger = SetLogger(logger);
    private readonly ConfigXmlParser<EngineConfigXml> _configXmlParser = new(SetLogger(logger), XmlNameSpaces.ConfigEngine, Paths.Xsd.XsdEngineConfigDto);
    private readonly PluginLoader _pluginLoader = new(SetLogger(logger));
    private readonly EngineServiceBuilder _engineServiceBuilder = new(SetLogger(logger));
    
    private EngineConfigXml? _engineConfigDto;
    public EngineConfigXml EngineConfigDto {
        get => _engineConfigDto ??= new EngineConfigXml().PopulateAsEmpty();
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
            _engineConfigFlag |= EngineConfigFlags.PluginLoadOrderUnstable;
            _logger.Warning("Assembly could not be assigned as a Plugin");
            return this;
        }
        
        _pluginLoader.InjectAssemblyAsPlugin(assembly);
        return this;
    }
    
    public EngineConfiguration AssignDefaultServices() {
        // Systems which may be overriden
        _engineServiceBuilder.AssignFromServiceDescriptors([
            NewServiceDescriptor<ILogger>(EngineLogger.CreateLogger()),
            NewServiceDescriptor<IFrameProcessor, FrameProcessor>(ServiceLifetime.Singleton),
            NewServiceDescriptor<IMainWindow, MainWindow>(ServiceLifetime.Singleton)
        ]);
        
        _logger.Information("Assigned Default Systems correctly");
        _engineConfigFlag |= EngineConfigFlags.AssignedDefaultServices;
        return this;
    }
    
    public EngineConfiguration AssignStaticServices() {
        // TODO make a check though the _engineConfigFlag to see if everything has been setup already
        
        // services which may not be overriden
        _engineServiceBuilder.AssignFromServiceDescriptors([
            NewServiceDescriptor<IAssetAtlas, AssetAtlas>(ServiceLifetime.Singleton),
            NewServiceDescriptor<IEngine, Engine>(ServiceLifetime.Singleton),
            NewServiceDescriptor<IPluginAtlas, PluginAtlas>(ServiceLifetime.Singleton),
        ]);
        
        _logger.Information("Assigned Static Systems correctly");
        _engineConfigFlag |= EngineConfigFlags.AssignedStaticServices;
        return this;
    }

    public EngineConfiguration AddCustomServices(params ServiceDescriptor[] serviceDescriptor) {
        _engineServiceBuilder.AssignFromServiceDescriptors(serviceDescriptor);
        return this;
    }
    
    public EngineConfiguration ImportEngineConfig(string filePath, bool outputToLog = true) {
        // TODO make a check though the _engineConfigFlag to see if everything has been setup already
        
        // services which may not be overriden
        if (!_configXmlParser.TryDeserializeFromFile(filePath, out EngineConfigXml? configDto)) {
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
        
        EngineConfigDto.PluginData.LoadOrder.Plugins
            .IterateOver(p => p.FilePath = Path.Join(EngineConfigDto.PluginData.RootFolder, p.FileNameInternal));

        string[] filePaths = EngineConfigDto.PluginData.LoadOrder.Plugins.Select(p => p.FilePath).ToArray();
        
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
            .SelectMany<IPluginDto, ServiceDescriptor>(p => 
                p.GetServices()
                    .Concat(p.GetNexitiesComponents())
                    .Concat(p.GetNexitiesEntities())
            )
            .IterateOver(_engineServiceBuilder.AssignFromServiceDescriptor);
        
        _logger.Information("Assigned Systems from Plugins");
        _engineConfigFlag |= EngineConfigFlags.ImportedPluginServices;
        return this;
    }

    public EngineConfiguration AssignDependencyInjectionContainer() {
        // TODO make a check though the _engineConfigFlag to see if everything has been setup already
        if (!_engineConfigFlag.HasFlag(EngineConfigFlags.AssignedDefaultServices 
                                       | EngineConfigFlags.AssignedStaticServices)) {
            _logger.Error("No Default- or Static Engine Systems were assigned");
            throw new InvalidOperationException("No Default or Static Engine Systems were assigned");
        }

        _engineServiceBuilder.FinishBuilding();
        
        _logger.Information("DI Container built");
        _engineConfigFlag |= EngineConfigFlags.DiContainerBuilt;
        return this;
        
    }

    public IEngine CreateEngine() {
        if (_engineConfigFlag == EngineConfigFlags.UnConfigured) {
            // TODO throw proper error
            throw new Exception();
        }

        if (_engineConfigFlag.HasFlag(EngineConfigFlags.PluginLoadOrderUnstable) 
            && EngineConfigDto.PluginData.LoadOrder.BreakOnUnstable) {
            _logger.ThrowFatal<ArgumentException>("Engine could not be created -> Load Order was Unstable & configuration.BreakOnUnstable was set to {bool}", EngineConfigDto.PluginData.LoadOrder.BreakOnUnstable);
        }
        
        // Populate Plugin Atlas with plugin list
        //      Is a singleton anyway, so doesn't matter when we assign this data
        IPluginAtlas pluginAtlas = EngineServices.GetPluginAtlas();
        pluginAtlas.ImportPlugins(_pluginLoader.ExportToPlugins());
        
        // Create the Actual Engine
        //  Should be the last step
        IEngine engine = EngineServices.GetEngine();
        _logger.Information("Engine instance created of type: {Type}", engine.GetType().FullName);
        return engine;
    }
}