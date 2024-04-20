// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

using Serilog;

namespace AterraCore.Loggers;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------

// A "special" logger to be used before the service collection is built
//      Don't use this after the startup procedure has ended.
//      
public static class StartupLogger {
    public static ILogger CreateLogger() {
        return new LoggerConfiguration()
            .WriteTo.Console()
            .WriteTo.File("log-startup.txt")
            .CreateLogger();
    } 
}