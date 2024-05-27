// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using Serilog;
namespace AterraCore.Loggers;

using Common.Data;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public static class EngineLogger {
    public static LoggerConfiguration CreateConfiguration(bool asyncConsole) =>
        new LoggerConfiguration()
            .MinimumLevel.Verbose()
            .DefaultEnrich("Engine")
            .AsyncSinkFile(Paths.Logs.EngineLog)
            .SetConsole(asyncConsole);

    public static ILogger CreateLogger(bool asyncConsole) => CreateConfiguration(asyncConsole).CreateLogger();
}