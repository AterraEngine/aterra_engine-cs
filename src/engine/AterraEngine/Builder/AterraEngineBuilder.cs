// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Common.Attributes;
using AterraEngine.Contracts.Builder;
using AterraEngine.Contracts.Engine;
using AterraEngine.Core.DependencyInjection;
using AterraEngine.Loggers;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Core;
using Serilog.Events;

namespace AterraEngine.Builder;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[InjectableService<IAterraEngineBuilder>(ServiceLifetime.Singleton)]
public class AterraEngineBuilder : IAterraEngineBuilder {
    public IServiceCollection Services { get; } = CreateDefaultServices();
    public ILogger Logger { get; private set; } = LoggerConfigurations.CreateBuilderLogger(LogEventLevel.Verbose);
    
    // -----------------------------------------------------------------------------------------------------------------
    // Constructor Helpers
    // -----------------------------------------------------------------------------------------------------------------
    private static ServiceCollection CreateDefaultServices() {
        var services = new ServiceCollection();
        Logger logger = LoggerConfigurations.CreateEngineLogger(LogEventLevel.Verbose);
        services
            .AddSingleton(logger)
            .AddSingleton<ILogger>(logger)
        ;
        
        // Add auto generated services from various subprojects.
        services.RegisterServicesFromAterraEngine();
        
        return services;
    } 
    
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    
    #region Logging
    /// <inheritdoc />
    public IAterraEngineBuilder WithSerilog(ILogger logger) {
        Logger = logger;
        return this;
    }
    /// <inheritdoc />
    public IAterraEngineBuilder WithSerilog(Action<LoggerConfiguration> configure) {
        var configuration = new LoggerConfiguration();
        configure(configuration);
        Logger = configuration.CreateLogger();
        return this;
    }

    /// <inheritdoc />
    public IAterraEngineBuilder AddSerilog(ILogger logger) {
        Services.AddSingleton(logger);
        return this;
    }

    /// <inheritdoc />
    public IAterraEngineBuilder AddSerilog(Action<LoggerConfiguration> configure) {
        var configuration = new LoggerConfiguration();
        configure(configuration);
        Logger logger = configuration.CreateLogger();
        Services.AddSingleton(logger);
        Services.AddSingleton<ILogger>(logger);
        return this;
    }
    #endregion
    
    #region Build
    /// <inheritdoc />
    public Task<IAterraEngine> BuildAsync() => BuildAsync<IAterraEngine>();
    /// <inheritdoc />
    public async Task<T> BuildAsync<T>() where T : IAterraEngine {
        Logger.Information("Building engine...");
        
        // Todo Collect from source
        EngineServices.ConfigureServices(Services, []);
        
        Logger.Information("Finished building engine");
        return EngineServices.Provider.GetRequiredService<T>();
    }
    #endregion
}