// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Contracts.Builder.BootOperations;
using Microsoft.Extensions.DependencyInjection;

namespace AterraEngine.Builder.BootOperations.Chains.PluginExtraction;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public readonly struct PluginExtractionVariables(string chainName) : IChainVariables {
    public string ChainName { get; } = chainName;
    public IServiceCollection Services { get; } = new ServiceCollection();
}
