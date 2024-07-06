// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Common.Data;
using Serilog;

namespace AterraCore.Loggers;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public static class EngineLogger {
    private static LoggerConfiguration CreateConfiguration(bool asyncConsole) =>
        new LoggerConfiguration()
            .MinimumLevel.Verbose()
            .DefaultEnrich("Engine")
            .AsyncSinkFile(Paths.Logs.EngineLog)
            .SetConsole(asyncConsole);

    public static ILogger CreateLogger(bool asyncConsole) => CreateConfiguration(asyncConsole).CreateLogger();
}
