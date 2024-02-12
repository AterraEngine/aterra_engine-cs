// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using Serilog;

namespace AterraEngine.Core.Startup;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------

// A "special" logger to be used before the service collection is built
//      Don't use this after the startup procedure has ended.
//      
public static class StartupLogger {
    public static ILogger CreateLogger() => new LoggerConfiguration()
        .WriteTo.Console()
        .WriteTo.File("log-startup.txt")
        .CreateLogger();
}