// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Common.Data;
using Serilog;

namespace AterraCore.Loggers;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
/// <summary>
///     The EngineLogger class provides methods to create basic loggers for the engine.
/// </summary>
public static class EngineLogger {
    /// <summary>
    ///     Creates a logger configuration for the engine.
    /// </summary>
    /// <param name="asyncConsole">Indicates whether the console logging should be asynchronous.</param>
    /// <returns>The logger configuration.</returns>
    /// <remarks>
    ///     The logger configuration includes the minimum log level, default enrichers,
    ///     and sinks for file logging and console logging.
    /// </remarks>
    private static LoggerConfiguration CreateConfiguration(bool asyncConsole) =>
        new LoggerConfiguration()
            .MinimumLevel.Verbose()
            .DefaultEnrich("Engine")
            .AsyncSinkFile(Paths.Logs.EngineLog)
            .SetConsole(asyncConsole, LoggerConfigurationExtensions.OutputTemplateStartup);

    /// <summary>
    ///     Creates a logger based on the provided configuration.
    /// </summary>
    /// <param name="asyncConsole">Specify whether to enable asynchronous console logging.</param>
    /// <returns>The created logger.</returns>
    public static ILogger CreateLogger(bool asyncConsole) => CreateConfiguration(asyncConsole).CreateLogger();
}
