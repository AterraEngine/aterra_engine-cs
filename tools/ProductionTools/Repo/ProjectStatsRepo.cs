// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Collections.Concurrent;

namespace ProductionTools.Repo;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class ProjectStatsRepo(ILogger logger) {
    #region GetAllFilePaths
    public ConcurrentBag<string> GetAllFilePaths(string basePath, string[] searchPatterns) {
        var filePaths = new ConcurrentBag<string>();
        
        Parallel.ForEach(searchPatterns, body: pattern => {
            foreach (string file in Directory.GetFiles(basePath, pattern, SearchOption.AllDirectories)) {
                filePaths.Add(file);
            }
        });
        
        logger.Debug($"Found {filePaths.Count} file entries");
        return filePaths;
    }
    #endregion
    #region GetAllLineCounts
    public async Task<ConcurrentBag<int>> GetAllLineCountsAsync(string basePath, string[] searchPatterns, CancellationToken cancellationToken = default) =>
        await GetAllLineCountsAsync(GetAllFilePaths(basePath, searchPatterns), cancellationToken);
    public async Task<ConcurrentBag<int>> GetAllLineCountsAsync(IEnumerable<string> filePaths, CancellationToken cancellationToken = default) {
        var lineCounts = new ConcurrentBag<int>();
        
        await Parallel.ForEachAsync(filePaths, cancellationToken, async (filepath, ct) => {
                lineCounts.Add((await File.ReadAllLinesAsync(filepath, ct)).Length);
            }
        );
        logger.Debug($"Found {lineCounts.Count} line entries");
        return lineCounts;
    }
    #endregion
    #region GetAllCharCounts
    public async Task<ConcurrentBag<int>> GetAllCharCountsAsync(string basePath, string[] searchPatterns, CancellationToken cancellationToken = default) =>
        await GetAllCharCountsAsync(GetAllFilePaths(basePath, searchPatterns), cancellationToken);
    public async Task<ConcurrentBag<int>> GetAllCharCountsAsync(IEnumerable<string> filePaths, CancellationToken cancellationToken = default) {
        var charCounts = new ConcurrentBag<int>();
        
        await Parallel.ForEachAsync(filePaths, cancellationToken,
            async (filepath, token) => {
                using var reader = new StreamReader(filepath);
                while (await reader.ReadLineAsync(token) is {} line) {
                    charCounts.Add(line.Length);
                }
            }
        );
        logger.Debug($"Found {charCounts.Count} chars entries");
        return charCounts;
    }
    #endregion
}
