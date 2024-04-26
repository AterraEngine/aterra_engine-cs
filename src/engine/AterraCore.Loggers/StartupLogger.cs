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
    public static LoggerConfiguration CreateConfiguration() {
        return new LoggerConfiguration()
            .MinimumLevel.Debug()

            .DefaultEnrich("Startups")
            .AsyncSinkFile(Paths.Logs.StartupLog)
            .AsyncSinkConsole();
    }
    
    public static ILogger CreateLogger() => CreateConfiguration().CreateLogger();
}