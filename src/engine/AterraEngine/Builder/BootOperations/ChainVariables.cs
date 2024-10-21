// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Contracts.Builder.BootOperations;
using Microsoft.Extensions.DependencyInjection;

namespace AterraEngine.Builder.BootOperations;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public readonly struct ChainVariables(string chainName) : IChainVariables {
    public string ChainName { get; } = chainName;
    public IServiceCollection Services { get; } = new ServiceCollection();
}
