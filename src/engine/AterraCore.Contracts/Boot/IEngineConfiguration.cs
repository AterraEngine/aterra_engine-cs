// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Common.Data;
using AterraCore.Contracts.DI;
using Serilog;

namespace AterraCore.Contracts.Boot;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------

public interface IEngineConfiguration : IBootConfiguration {
    public BootFlowOfOperations Flow { get; }
    public ILogger StartupLog { get; }
    public Func<ILogger> EngineLoggerCallback { get; set; }
    public IEngineServiceBuilder EngineServiceBuilder { get; }
    public ISubConfigurations SubConfigurations { get; set; }

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public IEngineConfiguration UseDefaultEngine();
    public IEngineConfiguration UseCustomEngine<T>() where T : IEngine;
    public IEngine CreateEngine();



















}