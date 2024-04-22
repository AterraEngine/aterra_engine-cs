// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

using AterraCore.Common;
using Serilog;
using Serilog.Formatting.Json;

namespace AterraCore.Loggers;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public static class EngineLogger {
    public static ILogger CreateLogger() {
        // create a custom formatter (like the JSON formatter) for easy parsing and analytics

        return new LoggerConfiguration()
            .Enrich.FromLogContext()
            .Enrich.WithProperty("Application", "AterraEngine")
            .Enrich.WithProperty("Stage", "Engine")
            .Enrich.WithProperty("MachineName", Environment.MachineName)
                
            // Using Async Sink to write logs asynchronously 
            // to avoid any performance issues during gameplay
            .WriteTo.Async(lc => lc.SQLite(
                Paths.EngineLog
            ))
            
            .WriteTo.Async(lc => lc.Console())
            
            .CreateLogger();
    
    }
}