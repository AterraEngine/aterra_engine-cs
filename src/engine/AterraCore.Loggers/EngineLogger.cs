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
                path: Paths.Logs.EngineLog,
                rollingInterval: RollingInterval.Day
            ))
            
            .WriteTo.Async(lc => lc.Console(
                outputTemplate:"[{Timestamp:HH:mm:ss.fff} {Level:u3}] {Message:lj}{NewLine}{Exception}"
                ))
            
            .CreateLogger();
    
    }
}