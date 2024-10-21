// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Contracts.Builder.BootOperations;
using AterraEngine.Contracts.Engine;
using Serilog;

namespace AterraEngine.Contracts.Builder;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
/// <summary>
/// Interface for building an AterraEngine with configurable logging options.
/// </summary>
public interface IAterraEngineBuilder {
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    
    #region Serilogger
    /// <summary>
    /// Configures the builder to use the specified Serilog logger.
    /// </summary>
    /// <param name="logger">An instance of the ILogger to be used by the builder.</param>
    /// <returns>A reference to the IAterraEngineBuilder instance for method chaining.</returns>
    IAterraEngineBuilder WithSerilog(ILogger logger);
    
    /// <summary>
    /// Configures the builder to use the specified Serilog logger.
    /// </summary>
    /// <param name="configure">An action to configure the LoggerConfiguration for Serilog.</param>
    /// <returns>A reference to the IAterraEngineBuilder instance for method chaining.</returns>
    IAterraEngineBuilder WithSerilog(Action<LoggerConfiguration> configure);
    
    /// <summary>
    /// Adds Serilog Services as the Engine Logging provider
    /// </summary>
    /// <param name="logger">An instance of Serilog's ILogger to be used by the engine.</param>
    /// <returns>Returns the IAterraEngineBuilder instance with the configured Serilog logger.</returns>
    IAterraEngineBuilder AddSerilog(ILogger logger);

    /// <summary>
    /// Adds Serilog Services as the Engine Logging provider.
    /// </summary>
    /// <param name="configure">An action to configure the LoggerConfiguration for Serilog.</param>
    /// <returns>The current IAterraEngineBuilder instance for method chaining.</returns>
    IAterraEngineBuilder AddSerilog(Action<LoggerConfiguration> configure);
    #endregion

    #region Build
    /// <summary>
    /// Builds the IAterraEngine instance.
    /// </summary>
    /// <returns>An instance of IAterraEngine constructed based on the configured settings.</returns>
    Task<IAterraEngine> BuildAsync();
    
    /// <summary>
    /// Builds the engine with the current configuration.
    /// </summary>
    /// <returns>An instance of <see cref="IAterraEngine"/>.</returns>
    Task<T> BuildAsync<T>() where T : IAterraEngine;
    #endregion
}
