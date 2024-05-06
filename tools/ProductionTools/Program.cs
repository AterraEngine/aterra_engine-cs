﻿// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

using AterraCore.Loggers;
using CliArgsParser;
using CliArgsParser.Contracts;
using ProductionTools.Commands;
using Serilog;
using Serilog.Core;

namespace ProductionTools;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------

public static class Program {
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    private static async Task MainAsync(string[] args) {
        await using Logger logger = new LoggerConfiguration()
            .MinimumLevel.Verbose()
            .DefaultSinkConsole() // Using the normal version of the Sink Console, else the empty lines get processed earlier.
            .CreateLogger();

        IParser parser = new ParserConfiguration()
            .SetLogger(logger)
            .RegisterAtlas(new XmlSchemaGenerator(logger))
            .RegisterAtlas(new TestConsoleTheme(logger))
            .CreateArgsParser();

        await parser.TryParseAsync(string.Join(' ', args));
    }
    
    public static void Main(string[] args) {
        MainAsync(args).GetAwaiter().GetResult();
    }
}