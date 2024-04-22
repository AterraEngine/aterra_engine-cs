// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

using AterraCore.Common;
using Serilog;

namespace AterraCore.Loggers;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------

// A "special" logger to be used before the service collection is built
//      Don't use this after the startup procedure has ended.
public static class StartupLogger {
    public static ILogger CreateLogger() {
        return new LoggerConfiguration()
            .Enrich.FromLogContext()
            .Enrich.WithProperty("Application", "AterraEngine")
            .Enrich.WithProperty("Stage", "Engine")
            .Enrich.WithProperty("MachineName", Environment.MachineName)
                
            // Using Async Sink to write logs asynchronously 
            // to avoid any performance issues during gameplay
            .WriteTo.Async(lc => lc.SQLite(
                Paths.StartupLog
            ))
            
            .WriteTo.Async(lc => lc.Console())
            
            .CreateLogger();
    } 
}