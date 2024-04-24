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

// A "special" logger to be used before the service collection is built
//      Don't use this after the startup procedure has ended.
public static class StartupLogger {
    public static ILogger CreateLogger() {
        return new LoggerConfiguration()
            .MinimumLevel.Debug() 
            
            .Enrich.FromLogContext()
            .Enrich.WithProperty("Application", "AterraEngine")
            .Enrich.WithProperty("Stage", "Engine")
            .Enrich.WithProperty("MachineName", Environment.MachineName)
            .Enrich.WithThreadId()
            .Enrich.WithMemoryUsage()
                
            // Using Async Sink to write logs asynchronously 
            // to avoid any performance issues during gameplay
            .WriteTo.Async(lc => lc.File(
                formatter: new CompactJsonFormatter(), 
                path: Paths.Logs.StartupLog,
                rollingInterval: RollingInterval.Day
            ))
            
            .WriteTo.Async(lc => lc.Console(
                outputTemplate:"[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}"
            ))
            
            .CreateLogger();
    } 
}