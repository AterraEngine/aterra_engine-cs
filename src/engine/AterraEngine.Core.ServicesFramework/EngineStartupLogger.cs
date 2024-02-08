// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using Serilog;
using Serilog.Core;

namespace AterraEngine.Core.ServicesFramework;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------

// A "special" logger to be used before the service collection is built
//      Don't use this after the startup procedure has ended.
//      
public static class EngineStartupLogger {
    public static readonly Logger Log = new LoggerConfiguration()
        .WriteTo.Console()
        .WriteTo.File("log-startup.txt")
        .CreateLogger();
    
}