// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Common;
using AterraCore.Common.Config;
using AterraCore.Contracts;
using AterraCore.Contracts.FlexiPlug;
using AterraCore.Contracts.FlexiPlug.Plugin;
using AterraCore.Contracts.Nexities.Assets;
using AterraCore.Contracts.Nexities.Worlds;
using AterraCore.Contracts.Renderer;
using AterraCore.DI;
using AterraCore.FlexiPlug;
using AterraCore.Loggers;
using AterraCore.Nexities.Assets;
using AterraCore.Nexities.Worlds;
using AterraEngine.Config;
using AterraEngine.Renderer.RaylibCs;
using AterraEngine.Renderer.RaylibCs.FrameProcessors;
using AterraEngine.Threading;
using Extensions;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Core;
using Xml;
using static AterraEngine.Configuration.ConfigurationWarnings;
using static AterraEngine.Configuration.FlowOfOperations;
using static Extensions.ServiceDescriptorExtension;

namespace AterraEngine.Configuration;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------

public class EngineConfiguration(ILogger? logger = null) {
    private readonly ILogger _logger = GetStartupLogger(logger);
    private readonly EngineServiceBuilder _engineServiceBuilder = new(GetStartupLogger(logger));
    
    private EngineConfigXml? _engineConfigDto;
    public EngineConfigXml EngineConfigDto {
        get => _engineConfigDto ??= new EngineConfigXml().PopulateAsEmpty();
        set => _engineConfigDto = value;
    }

    private ServiceDescriptor? _serviceDescriptorCache;
    public ServiceDescriptor EngineLoggerDescriptor {
        get => _serviceDescriptorCache ??= NewServiceDescriptor<ILogger>(Logger.None);
        set => _serviceDescriptorCache = value;
    }
    
    private ConfigurationWarnings _configurationWarnings = Nominal;
    private FlowOfOperations _flow = UnConfigured;
    
    private LinkedList<IPlugin>? _plugins;
    // TODO separate EngineConfigFlags into "flow of operations" & "errors"

    private static ILogger GetStartupLogger(ILogger? logger) => logger ?? StartupLogger.CreateLogger(false);

    // -----------------------------------------------------------------------------------------------------------------
    // Configuration Methods
    // -----------------------------------------------------------------------------------------------------------------

    public EngineConfiguration SetEngineLogger(Func<bool, ILogger> loggerCallback) {
        EngineLoggerDescriptor = NewServiceDescriptor<ILogger>(loggerCallback(EngineConfigDto.Logging.UseAsyncConsole));
        return this;
    }
    
    public EngineConfiguration AssignDefaultServices() {
        if (_flow > AssignedDefaultServices) {
            // TODO write exception
            _configurationWarnings |= FlowOfOperationsNotRespected;
        }
        // Systems which may be overriden
        _engineServiceBuilder.AssignFromServiceDescriptors([
            EngineLoggerDescriptor,
            // NewServiceDescriptor<IFrameProcessor, AbstractRaylibFrameProcessor>(ServiceLifetime.Singleton),
            NewServiceDescriptor<IMainWindow, MainWindow>(ServiceLifetime.Singleton)
        ]);
        
        _logger.Information("Assigned Default Systems correctly");
        _flow = AssignedStaticServices;
        return this;
    }
    
    public EngineConfiguration AssignStaticServices() {
        if (_flow > AssignedStaticServices) {
            // TODO write exception
            _configurationWarnings |= FlowOfOperationsNotRespected;
        }
        
        // services which may not be overriden
        _engineServiceBuilder.AssignFromServiceDescriptors([
            NewServiceDescriptor<IAssetAtlas, AssetAtlas>(ServiceLifetime.Singleton),
            NewServiceDescriptor<IAssetInstanceAtlas, AssetInstanceAtlas>(ServiceLifetime.Singleton),
            NewServiceDescriptor<IEngine, Engine>(ServiceLifetime.Singleton),
            NewServiceDescriptor<IPluginAtlas, PluginAtlas>(ServiceLifetime.Singleton),
            NewServiceDescriptor<IWorld, World>(ServiceLifetime.Singleton),
            NewServiceDescriptor<RenderThreadEvents, RenderThreadEvents>(ServiceLifetime.Singleton),
            NewServiceDescriptor<IApplicationStageManager, ApplicationStageManager>(ServiceLifetime.Singleton),
        ]);
        
        _logger.Information("Assigned Static Systems correctly");
        _flow = AssignedStaticServices;
        return this;
    }

    public EngineConfiguration AddCustomServices(params ServiceDescriptor[] serviceDescriptor) {
        _engineServiceBuilder.AssignFromServiceDescriptors(serviceDescriptor);
        return this;
    }
    
    public EngineConfiguration ImportEngineConfig(string filePath, bool outputToLog = true) {
        if (_flow > ImportedEngineConfigDto) {
            // TODO write exception
            _configurationWarnings |= FlowOfOperationsNotRespected;
        }
        
        ConfigXmlParser<EngineConfigXml> configXmlParser = new(_logger, XmlNameSpaces.ConfigEngine, Paths.Xsd.XsdEngineConfigDto);
        if (!configXmlParser.TryDeserializeFromFile(filePath, out EngineConfigXml? configDto)) {
            _logger.Error("Engine config file could not be parsed");
            return this; //the _configDto will be null, so setter of EngineConfigDto will populate as empty
        }

        EngineConfigDto = configDto;
        if (outputToLog) EngineConfigDto.OutputToLog(_logger);

        _logger.Information("Imported Engine Config correctly");
        _flow = ImportedEngineConfigDto;
        return this;
    }

    public EngineConfiguration WithPluginConfiguration(Func<PluginConfiguration, LinkedList<IPlugin>> callback) {
        if (_flow > ImportedPlugins) {
            _configurationWarnings |= FlowOfOperationsNotRespected;
        }
        
        EngineConfigDto.PluginData.LoadOrder.Plugins
            .IterateOver(p => p.FilePath = Path.Join(EngineConfigDto.PluginData.RootFolder, p.FileNameInternal));
        string[] filePaths = EngineConfigDto.PluginData.LoadOrder.Plugins.Select(p => p.FilePath).ToArray();
        _logger.Information("All plugin file paths: {paths}", filePaths);

        var pluginConfiguration = new PluginConfiguration(_logger, filePaths, _engineServiceBuilder);
        _plugins = callback(pluginConfiguration);
        
        // TODO review if this if structure is logical
        if (_plugins is not null && pluginConfiguration.ConfigurationWarnings.IsNotNominal()) {
            // TODO plugins were loaded, but something went wrong during setup 
            _configurationWarnings |= pluginConfiguration.ConfigurationWarnings;
        }

        if (_plugins is null) {
            // TODO everything went correctly, but no plugins were loaded, aka none were defined?
            _configurationWarnings |= NoPluginsDefined;
        }

        _flow = ImportedPlugins;
        return this;
    }

    public EngineConfiguration BuildDependencyInjectionContainer() {
        // TODO make a check though the _engineConfigFlag to see if everything has been setup already
        if (_flow < AssignedStaticServices) {
            _configurationWarnings |= FlowOfOperationsNotRespected;
            
            _logger.Error("No Static Engine Systems were assigned");
            throw new InvalidOperationException("No Default or Static Engine Systems were assigned");
        }

        _engineServiceBuilder.FinishBuilding();
        
        _logger.Information("DI Container built");
        _flow = DiContainerBuilt;
        return this;
        
    }

    public IEngine CreateEngine() {
        if (_flow == UnConfigured) {
            _logger.ThrowFatal<InvalidOperationException>("Engine was not correctly configured");
        }

        if (_configurationWarnings.HasFlag(PluginLoadOrderUnstable) && EngineConfigDto.PluginData.LoadOrder.BreakOnUnstable) {
            _logger.ThrowFatal<ArgumentException>("Engine could not be created -> Load Order was Unstable & configuration.BreakOnUnstable was set to {bool}", EngineConfigDto.PluginData.LoadOrder.BreakOnUnstable);
        }

        if (_configurationWarnings.HasFlag(NoPluginsDefined) || _plugins is null) {
            _logger.ThrowFatal<InvalidOperationException>("No Plugins were defined");
        }
        
        // Populate Plugin Atlas with plugin list
        //      Is a singleton anyway, so doesn't matter when we assign this data
        IPluginAtlas pluginAtlas = EngineServices.GetPluginAtlas();
        pluginAtlas.ImportPlugins(_plugins);
        
        // Create the Actual Engine
        //  Should be the last step
        IEngine engine = EngineServices.GetEngine();
        _logger.Information("Engine instance created of type: {Type}", engine.GetType().FullName);
        return engine;
    }
}