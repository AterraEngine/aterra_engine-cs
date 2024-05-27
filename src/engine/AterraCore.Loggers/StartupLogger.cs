﻿// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

using Serilog;
namespace AterraCore.Loggers;

using Common.Data;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------

// A "special" logger to be used before the service collection is built
//      Don't use this after the startup procedure has ended.
public static class StartupLogger {
    private static ILogger? LoggerCache { get; set; }

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public static LoggerConfiguration CreateConfiguration(bool asyncConsole) =>
        new LoggerConfiguration()
            .MinimumLevel.Verbose()
            .DefaultEnrich("Startup")
            .AsyncSinkFile(Paths.Logs.StartupLog)
            .SetConsole(asyncConsole);

    public static ILogger CreateLogger(bool asyncConsole) => LoggerCache ??= CreateConfiguration(asyncConsole).CreateLogger();
}