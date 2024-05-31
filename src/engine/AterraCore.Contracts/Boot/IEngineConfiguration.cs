// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Common.ConfigFiles.EngineConfig;
using AterraCore.Common.Data;
using AterraCore.Contracts.Boot.FlexiPlug;
using AterraCore.Contracts.Boot.FlexiPlug.PluginDtos;
using AterraCore.Contracts.Boot.Nexities;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace AterraCore.Contracts.Boot;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------

public interface IEngineConfiguration : 
    IHasSubConfiguration<INexitiesConfiguration>,
    IHasSubConfiguration<IFlexiPlugConfiguration>
{
    public ConfigurationWarnings ConfigurationWarnings { get; set; }
    public BootFlowOfOperations Flow { get; }
    public ILogger StartupLog { get; }
    public EngineConfigXml EngineConfigDto { get; }

    public IFlexiPlugConfiguration FlexiPlug { get; }
    public INexitiesConfiguration Nexities { get; }

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public IEngineConfiguration SetEngineLogger(Func<ILogger> loggerCallback);
    public IEngineConfiguration SetFlexiPlugConfiguration(IFlexiPlugConfiguration configuration);
    public IEngineConfiguration SetNexitiesConfiguration(INexitiesConfiguration configuration);
    
    public IEngineConfiguration AssignDefaultServices(IEnumerable<ServiceDescriptor> serviceDescriptors);
    public IEngineConfiguration AssignStaticServices(IEnumerable<ServiceDescriptor> serviceDescriptors);
    public IEngineConfiguration ImportEngineConfig(string filePath, bool outputToLog = true);
    public IEngineConfiguration BuildDependencyInjectionContainer();
    
    public IEngine CreateEngine();



















}