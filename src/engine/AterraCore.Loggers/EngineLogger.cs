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
    public static ILogger CreateLogger() {
        return new LoggerConfiguration()
            .MinimumLevel.Debug() 
            
            .DefaultEnrich("Startup")
            .DefaultSinkFile(Paths.Logs.EngineLog)
            .DefaultSinkConsole()
            
            .CreateLogger();
    
    }
}