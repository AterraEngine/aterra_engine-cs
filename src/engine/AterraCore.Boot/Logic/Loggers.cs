// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Contracts.Boot;
using Serilog;

namespace AterraCore.Boot.Logic;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------

public static class Loggers {
    public static IEngineConfiguration SetEngineLogger(this IEngineConfiguration configuration, Func<ILogger> loggerCallback) {
        configuration.EngineLoggerCallback = loggerCallback;
        return configuration;
    }
}
