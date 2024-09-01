// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Extensions.Serilog;
using JetBrains.Annotations;
using ProductionTools.Repo;
using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;

namespace ProductionTools.Commands;
// ---------------------------------------------------------------------------------------------------------------------
// Support Code
// ---------------------------------------------------------------------------------------------------------------------
[SuppressMessage("ReSharper", "AutoPropertyCanBeMadeGetOnly.Global")]
[SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
public class StatCountArgsOptions : ICommandParameters {
    [ArgValue("path")] public string Path { get; set; } = "../../../../../../";
}

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class ProjectStats(ILogger logger, ProjectStatsRepo repo) : ICommandAtlas {
    // -----------------------------------------------------------------------------------------------------------------
    // Commands
    // -----------------------------------------------------------------------------------------------------------------
    [Command<StatCountArgsOptions>("stats-count")]
    [UsedImplicitly]
    public async Task StatsCounter(StatCountArgsOptions args) {

        string[] searchPatterns = ["*.cs"];
        // string[] searchPatterns = ["*.cs", "*.xsd"];
        ConcurrentBag<string> filePaths = repo.GetAllFilePaths(args.Path, searchPatterns);

        (ConcurrentBag<int> lineCounts, ConcurrentBag<int> charCounts) = await Task
            .WhenAll(repo.GetAllLineCountsAsync(filePaths), repo.GetAllCharCountsAsync(filePaths))
            .ContinueWith(t => (t.Result[0], t.Result[1]));

        string fileTypes = string.Join(" & ", searchPatterns.Select(p => p.Replace("*", "")));

        int totalLineCount = lineCounts.Sum();
        int totalCharCount = charCounts.Sum();
        int totalFileCount = filePaths.Count;

        logger.Information("Stats : Files ({fileTypes}) : {count}", fileTypes, totalFileCount);
        logger.Information("Stats : Lines ({fileTypes}) : {count}", fileTypes, totalLineCount);
        logger.Information("Stats : Chars ({fileTypes}) : {count}", fileTypes, totalCharCount);
    }
}
