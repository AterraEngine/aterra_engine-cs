// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Common.ConfigFiles.EngineConfig;
using AterraCore.Common.Data;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace AterraCore.Contracts.Boot;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------

public interface IEngineConfiguration {
    public ConfigurationWarnings ConfigurationWarnings { get; set; }
    public BootFlowOfOperations Flow { get; }
    public ILogger StartupLog { get; }
    public EngineConfigXml EngineConfigDto { get; }

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public IEngineConfiguration SetEngineLogger(Func<ILogger> loggerCallback);
    
    public IEngineConfiguration AssignDefaultServices(params ServiceDescriptor[] serviceDescriptors);
    public IEngineConfiguration AssignExtraServices(params ServiceDescriptor[] serviceDescriptors);
    public IEngineConfiguration RunSubConfigurations();
    public IEngineConfiguration AssignStaticServices(params ServiceDescriptor[] serviceDescriptors);
    public IEngineConfiguration ImportEngineConfig(string filePath, bool outputToLog = true);
    public IEngineConfiguration BuildDependencyInjectionContainer();
    
    public IEngine CreateEngine();



















}