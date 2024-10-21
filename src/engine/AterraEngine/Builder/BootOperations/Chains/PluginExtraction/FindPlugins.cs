// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Contracts.Builder.BootOperations;

namespace AterraEngine.Builder.BootOperations.Chains.PluginExtraction;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class FindPlugins : IChainOperation<PluginExtractionVariables> {
    public async Task<PluginExtractionVariables> ExecuteAsync(PluginExtractionVariables chainVariables, CancellationToken cancellationToken) {
        // do something
        
        Console.WriteLine("Find plugins");

        return chainVariables;
    }
}
