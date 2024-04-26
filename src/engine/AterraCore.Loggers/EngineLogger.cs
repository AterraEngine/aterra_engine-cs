// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Common;
using Serilog;
using Serilog.Formatting.Compact;

namespace AterraCore.Loggers;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public static class EngineLogger {
    public static LoggerConfiguration CreateConfiguration() {
        return new LoggerConfiguration()
            .MinimumLevel.Debug()

            .DefaultEnrich("Engine")
            .AsyncSinkFile(Paths.Logs.EngineLog)
            .AsyncSinkConsole();
    }
    
    public static ILogger CreateLogger() => CreateConfiguration().CreateLogger();
}