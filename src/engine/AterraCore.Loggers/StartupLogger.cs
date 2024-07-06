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
/// A "special" logger to be used before the service collection is built.
/// Don't use this after the startup procedure has ended.
/// </summary>
public static class StartupLogger {
    /// <summary>
    /// A "special" logger to be used before the service collection is built.
    /// Don't use this after the startup procedure has ended.
    /// </summary>
    private static ILogger? LoggerCache { get; set; }

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    
    /// <summary>
    /// Creates a <see cref="LoggerConfiguration"/> for the startup logger.
    /// </summary>
    /// <param name="asyncConsole">A boolean value indicating whether to use asynchronous console sink.</param>
    /// <returns>A <see cref="LoggerConfiguration"/> object.</returns>
    private static LoggerConfiguration CreateConfiguration(bool asyncConsole) =>
        new LoggerConfiguration()
            .MinimumLevel.Verbose()
            .DefaultEnrich("Startup")
            .AsyncSinkFile(Paths.Logs.StartupLog)
            .SetConsole(asyncConsole);

    /// <summary>
    /// Creates a logger instance with the specified configuration.
    /// </summary>
    /// <param name="asyncConsole">Indicates whether the console sink should be asynchronous.</param>
    /// <returns>A logger instance.</returns>
    public static ILogger CreateLogger(bool asyncConsole) => LoggerCache ??= CreateConfiguration(asyncConsole).CreateLogger();
}
