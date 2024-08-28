// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CliArgsParser;
using CodeOfChaos.Extensions.Serilog;
using JetBrains.Annotations;
using Serilog;
using System.Diagnostics.CodeAnalysis;

namespace ProductionTools.Commands;
// ---------------------------------------------------------------------------------------------------------------------
// Support Code
// ---------------------------------------------------------------------------------------------------------------------
[SuppressMessage("ReSharper", "AutoPropertyCanBeMadeGetOnly.Global")]
[SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
public class StatCountArgsOptions : ICommandParameters {
    [ArgValue("path")] public string Path { get; set; } = @"E:\Portfolio\internal\005-aterra_engine\0001-cs-aterra_engine";
}

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class ProjectStats(ILogger logger) : ICommandAtlas {
    // -----------------------------------------------------------------------------------------------------------------
    // Commands
    // -----------------------------------------------------------------------------------------------------------------
    [Command<StatCountArgsOptions>("stats-count")]
    [UsedImplicitly]
    public async Task StatsCounter(StatCountArgsOptions args) {

        string[] filePaths = Directory.GetFiles(args.Path, "*.cs", SearchOption.AllDirectories);
        int totalLineCount = 0;
        int totalFileCountCs = filePaths.Length;

        await Parallel.ForEachAsync(
            filePaths, body: async (filepath, ct) => {
                Interlocked.Add(ref totalLineCount, (await File.ReadAllLinesAsync(filepath, ct)).Length);
            }
        );

        var builder = new ValuedStringBuilder();
        builder.AppendLine("Stats");
        builder.AppendLineValued("- Files (.cs) : ", totalFileCountCs);
        builder.AppendLineValued("- Lines (.cs) : ", totalLineCount);

        logger.Information(builder);


    }
}
