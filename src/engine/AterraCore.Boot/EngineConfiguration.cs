// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Boot.FlexiPlug;
using AterraCore.Boot.Nexities;
using AterraCore.Common.ConfigFiles.EngineConfig;
using AterraCore.Common.Data;
using AterraCore.Contracts;
using AterraCore.Contracts.Boot;
using AterraCore.Contracts.FlexiPlug;
using AterraCore.DI;
using AterraEngine;
using Extensions;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Core;
using Xml;
using static AterraCore.Common.Data.BootFlowOfOperations;
using static AterraCore.Common.Data.ConfigurationWarnings;
using static Extensions.ServiceDescriptorExtension;

namespace AterraCore.Boot;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class EngineConfiguration(ILogger? logger = null) : IEngineConfiguration {
    private ConfigurationWarnings _configurationWarningsCache = Nominal;
    public ConfigurationWarnings ConfigurationWarnings {
        get => _configurationWarningsCache;
        set {
            if (EngineConfigDto.BreakOnFlowException) {
                StartupLog.ThrowFatal<InvalidOperationException>(
                "Engine was not correctly configured with the following flags {@flags}", 
                _configurationWarningsCache.GetFlaggedValues().Select(f => f.ToString())
                );
            }
            _configurationWarningsCache = value;
        }
    }

    public BootFlowOfOperations Flow { get; set; } = UnConfigured;
    
    private readonly EngineServiceBuilder _engineServiceBuilder = new(GetStartupLogger(logger));
    
    public ILogger StartupLog { get; } = GetStartupLogger(logger);
    private Func<ILogger> EngineLoggerCallback { get; set; } = () => Loggers.EngineLogger.CreateLogger(false);
    
    private EngineConfigXml? _engineConfigDto;
    public EngineConfigXml EngineConfigDto => _engineConfigDto ??= new EngineConfigXml();

    private FlexiPlugConfiguration FlexiPlugConfiguration { get; } = new(GetStartupLogger(logger));
    private NexitiesConfiguration NexitiesConfiguration { get; } = new(GetStartupLogger(logger));

    private IBootConfiguration[]? _bootConfigurationsCache;
    private IBootConfiguration[] BootSequenceExtenders => _bootConfigurationsCache ??= [
        FlexiPlugConfiguration,
        NexitiesConfiguration
    ];
    
    // -----------------------------------------------------------------------------------------------------------------
    // Private Helper Methods
    // -----------------------------------------------------------------------------------------------------------------

    // If the logger is already defined by the program.cs which creates the engine, use that one else use the standard
    private static ILogger GetStartupLogger(ILogger? logger) => logger ?? Loggers.StartupLogger.CreateLogger(false);

    // -----------------------------------------------------------------------------------------------------------------
    // Configuration Methods
    // -----------------------------------------------------------------------------------------------------------------
    public IEngineConfiguration SetEngineLogger(Func<ILogger> loggerCallback) {
        EngineLoggerCallback = loggerCallback;
        return this;
    }

    public IEngineConfiguration AssignDefaultServices(IEnumerable<ServiceDescriptor>? serviceDescriptors = null) {
        if (Flow > AssignedDefaultServices) ConfigurationWarnings |= FlowOfOperationsNotRespected;
        
        // Systems which may be overriden
        if (serviceDescriptors is not null) {
            _engineServiceBuilder.AssignFromServiceDescriptors(serviceDescriptors.Concat([
                NewServiceDescriptor<ILogger>(EngineLoggerCallback()), 
                .. BootSequenceExtenders.SelectMany(extender => extender.ServicesDefault)
            ]));
        }

        StartupLog.Information("Assigned Default Systems correctly");
        Flow = AssignedStaticServices;
        return this;
    }

    public IEngineConfiguration AssignStaticServices(IEnumerable<ServiceDescriptor>? serviceDescriptors = null) {
        if (Flow > AssignedStaticServices) ConfigurationWarnings |= FlowOfOperationsNotRespected;

        // services which may not be overriden
        if (serviceDescriptors is not null) {
            _engineServiceBuilder.AssignFromServiceDescriptors(serviceDescriptors.Concat([
                NewServiceDescriptor<IEngine, Engine>(ServiceLifetime.Singleton),
                .. BootSequenceExtenders.SelectMany(extender => extender.ServicesStatic)
            ]));
        }

        StartupLog.Information("Assigned Static Systems correctly");
        Flow = AssignedStaticServices;
        return this;
    }

    public IEngineConfiguration ImportEngineConfig(string filePath, bool outputToLog = true) {
        if (Flow > ImportedEngineConfigDto) ConfigurationWarnings |= FlowOfOperationsNotRespected;

        XmlParser<EngineConfigXml> configXmlParser = new(StartupLog, XmlNameSpaces.ConfigEngine, Paths.Xsd.XsdEngineConfigDto);
        if (!configXmlParser.TryDeserializeFromFile(filePath, out EngineConfigXml? configDto)) {
            StartupLog.Error("Engine config file could not be parsed");
            return this;//the _configDto will be null, so setter of EngineConfigDto will populate as empty
        }
        
        // import the EngineConfigDto
        _engineConfigDto = configDto;
        if (outputToLog) EngineConfigDto.OutputToLog(StartupLog);
        
        // 
        BootSequenceExtenders.IterateOver(
            extender => extender.ParseDataFromConfig(_engineConfigDto)
        );
        
        StartupLog.Information("Imported Engine PluginDtos correctly");
        Flow = ImportedEngineConfigDto;
        return this;
    }

    public IEngineConfiguration BuildDependencyInjectionContainer() {
        if (Flow < AssignedDefaultServices) AssignDefaultServices([]);
        if (Flow < AssignedStaticServices) AssignStaticServices([]);

        _engineServiceBuilder.FinishBuilding();

        StartupLog.Information("DI Container built");
        Flow = DiContainerBuilt;
        return this;

    }

    public IEngine CreateEngine() {
        if (Flow == UnConfigured) {
            StartupLog.ThrowFatal<InvalidOperationException>("Engine was not correctly configured");
        }

        if (ConfigurationWarnings.HasFlag(PluginLoadOrderUnstable) && EngineConfigDto.PluginData.LoadOrder.BreakOnUnstable) {
            StartupLog.ThrowFatal<ArgumentException>("Engine could not be created -> Load Order was Unstable & configuration.BreakOnUnstable was set to {bool}", EngineConfigDto.PluginData.LoadOrder.BreakOnUnstable);
        }

        if (ConfigurationWarnings.HasFlag(NoPluginsDefined)) {
            StartupLog.ThrowFatal<InvalidOperationException>("No Plugins were defined");
        }

        // Populate Plugin Atlas with plugin list
        //      Is a singleton anyway, so doesn't matter when we assign this data
        IPluginAtlas pluginAtlas = EngineServices.GetPluginAtlas();
        pluginAtlas.ImportLoadedPluginDtos(FlexiPlugConfiguration.PluginLoader.Plugins);

        // Create the Actual Engine
        //  Should be the last step
        IEngine engine = EngineServices.GetEngine();
        StartupLog.Information("Engine instance created of type: {Type}", engine.GetType().FullName);
        return engine;
    }
}
