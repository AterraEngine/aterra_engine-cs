﻿// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Loggers;
using Microsoft.Extensions.DependencyInjection;
using ProductionTools.Repo;
using Serilog.Core;

namespace ProductionTools;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------

public static class Program {
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    private async static Task Main(string[] args) {
        await using Logger logger = new LoggerConfiguration()
            .MinimumLevel.Verbose()
            .DefaultSinkConsole()// Using the normal version of the Sink Console, else the empty lines get processed earlier.
            .CreateLogger();

        IServiceCollection serviceCollection = new ServiceCollection();
        serviceCollection.AddSingleton<ILogger>(logger);
        serviceCollection.AddSingleton<ProjectStatsRepo>();

        serviceCollection.AddCliArgsParser(configuration =>
            configuration
                .SetConfig(new CliArgsParserConfig {
                    Overridable = true,
                    GenerateShortNames = true
                })
                .AddFromAssembly(typeof(Program).Assembly)
        );

        ServiceProvider provider = serviceCollection.BuildServiceProvider();

        if (args.Length > 0) {
            await provider.GetRequiredService<IArgsParser>().ParseAsyncLinear(args);
        } else {
            await provider.GetRequiredService<ICliParser>().StartParsingAsync();
        }
    }
}
