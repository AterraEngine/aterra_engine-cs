// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Common;
using Serilog;

namespace AterraCore.Loggers;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public static class EngineLogger {
    public static LoggerConfiguration CreateConfiguration() {
        return new LoggerConfiguration()
            .MinimumLevel.Verbose()

            .DefaultEnrich("Engine")
            .AsyncSinkFile(Paths.Logs.EngineLog)
            .AsyncSinkConsole();
    }
    
    public static ILogger CreateLogger() => CreateConfiguration().CreateLogger();
}