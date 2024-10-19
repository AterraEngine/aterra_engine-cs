// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using Serilog;
using Serilog.Core;
using Serilog.Events;

namespace AterraEngine.Loggers;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
/// Provides configuration methods for creating and managing loggers within the Aterra Engine.
/// /
public static class LoggerConfigurations {
    /// <summary>
    /// Creates a logger configuration for the engine.
    /// </summary>
    /// <param name="logEventLevel">The minimum log event level.</param>
    /// <returns>The logger configuration.</returns>
    public static LoggerConfiguration CreateEngineConfiguration(LogEventLevel logEventLevel) =>
        new LoggerConfiguration()
            .MinimumLevel.Is(logEventLevel)
            .AsyncSinkFile("logs/log_engine-.log")
            .SetConsole(
                allowAsync: true,
                LoggerConfigurationExtensions.OutputTemplateStartup
            )
    ;

    /// <summary>
    /// Creates a logger for the engine.
    /// </summary>
    /// <param name="logEventLevel">The minimum log event level.</param>
    /// <returns>The configured logger.</returns>
    public static Logger CreateEngineLogger(LogEventLevel logEventLevel) => 
        CreateEngineConfiguration(logEventLevel)
            .CreateLogger();

    /// <summary>
    /// Creates a <see cref="LoggerConfiguration" /> for the builder logger.
    /// </summary>
    /// <param name="logEventLevel">The minimum event level to capture.</param>
    /// <returns>A <see cref="LoggerConfiguration" /> object.</returns>
    public static LoggerConfiguration CreateBuilderConfiguration(LogEventLevel logEventLevel) =>
        new LoggerConfiguration()
            .MinimumLevel.Is(logEventLevel)
            .DefaultEnrich("Startup")
            .AsyncSinkFile("logs/log_builder-.log")
            .SetConsole(
                allowAsync: false,
                LoggerConfigurationExtensions.OutputTemplateStartup
            )
        ;

    /// <summary>
    /// Creates a logger for the builder.
    /// </summary>
    /// <param name="logEventLevel">The logging event level.</param>
    /// <returns>A Logger object configured for the builder.</returns>
    public static Logger CreateBuilderLogger(LogEventLevel logEventLevel) => 
        CreateBuilderConfiguration(logEventLevel)
            .CreateLogger();
}

