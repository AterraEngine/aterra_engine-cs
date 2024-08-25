// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CliArgsParser.Attributes;
using CliArgsParser.Contracts;
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
public class StatCountArgsOptions : IParameters {
    [AutoArgValue("path")] public string Path { get; set; } = @"E:\Portfolio\internal\005-aterra_engine\0001-cs-aterra_engine";
}
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[CommandAtlas]
public class ProjectStats(ILogger logger) {
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
            filePaths, async (filepath, ct) => {
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