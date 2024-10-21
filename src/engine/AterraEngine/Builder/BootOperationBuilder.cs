// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Contracts.Builder;
using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;

namespace AterraEngine.Builder;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class BootOperationBuilder : IBootOperationBuilder {
    private ConcurrentDictionary<string,IBootOperationChain> BootOperationsChains { get; } = [];
    
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public bool TryRegister(string chainName, params Type[] types) {
        if (string.IsNullOrEmpty(chainName)) return false;

        if (!BootOperationsChains.TryGetValue(chainName, out IBootOperationChain? chain)) {
            return BootOperationChain.TryCreate(types, out chain) 
                && BootOperationsChains.TryAdd(chainName, chain);
        }

        foreach (Type type in types) {
            chain.BootOperations.AddLast(type);
        }
        return true;
    }
    
    public bool TryGetChain(string chainName, [NotNullWhen(true)] out IBootOperationChain? chain) => BootOperationsChains.TryGetValue(chainName, out chain);

    public bool TryAddToChain(string chainName, params Type[] types) {
        return !string.IsNullOrEmpty(chainName) 
            && BootOperationsChains.TryGetValue(chainName, out IBootOperationChain? chain)
            && chain.TryAddLastRange(types);
    }
    
    public IBootOperationBuilder Register(string chainName, params Type[] types) {
        if (!TryRegister(chainName, types)) throw new InvalidOperationException($"Failed to register boot operations for chain '{chainName}'.");
        return this;
    }
    
    public IBootOperationChain GetChain(string chainName) {
        if (!TryGetChain(chainName, out IBootOperationChain? chain)) throw new InvalidOperationException($"Failed to get boot operations for chain '{chainName}'.");
        return chain;
    }
    
    public IBootOperationBuilder AddToChain(string chainName, params Type[] types) {
        if (!TryAddToChain(chainName, types)) throw new InvalidOperationException($"Failed to add boot operations to chain '{chainName}'.");
        return this;
    }


}
