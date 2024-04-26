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
            .DefaultSinkFile(Paths.Logs.EngineLog)
            .DefaultSinkConsole();
    }
    
    public static ILogger CreateLogger() => CreateConfiguration().CreateLogger();
}