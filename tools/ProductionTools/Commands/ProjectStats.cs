// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CliArgsParser;
using CodeOfChaos.Extensions.Serilog;
using JetBrains.Annotations;
using Serilog;
using System.Collections.Concurrent;
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

        string[] searchPatterns = { "*.cs", "*.xsd" };
        var filePaths = new ConcurrentBag<string>();

        Parallel.ForEach(searchPatterns, body: pattern => {
            foreach (string file in Directory.GetFiles(args.Path, pattern, SearchOption.AllDirectories)) {
                filePaths.Add(file);
            }
        });

        int totalLineCount = 0;
        int totalFileCount = filePaths.Count;

        await Parallel.ForEachAsync(filePaths, body: async (filepath, ct) => {
            Interlocked.Add(ref totalLineCount, (await File.ReadAllLinesAsync(filepath, ct)).Length);
        });

        string fileTypes = string.Join(" & ", searchPatterns.Select(p => p.Replace("*", "")));
        var builder = new ValuedStringBuilder();
        builder.AppendLine("Stats");
        builder.AppendLineValued($"- Files ({fileTypes}) : ", totalFileCount);
        builder.AppendLineValued($"- Lines ({fileTypes}) : ", totalLineCount);

        logger.Information(builder.ToString(), builder.ValuesToArray());


    }
}
