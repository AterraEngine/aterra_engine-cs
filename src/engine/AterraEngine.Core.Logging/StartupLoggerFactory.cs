// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using Serilog;
using Serilog.Events;

namespace AterraEngine.Core.Logging;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public static class StartupLoggerFactory {
    public static ILogger CreateLogger(LogEventLevel? level = null) =>
        new LoggerConfiguration()
            .MinimumLevel.Is(level ?? LogEventLevel.Verbose)
            .WriteTo.Console()
            .WriteTo.File("log-startup.txt", 
                rollingInterval: RollingInterval.Day, 
                retainedFileCountLimit: 7, 
                fileSizeLimitBytes: 10_000_000
            )
        // #if DEBUG 
        //     .WriteTo.Debug() // For some reason this isn't working?
        // #endif
            .CreateLogger();
}