// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Contracts.Builder.BootOperations;
using System.Diagnostics.CodeAnalysis;

namespace AterraEngine.Builder.BootOperations;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class BootOperationConfig {
    public readonly Dictionary<string,IBootOperationChain> BootOperationsChains  = [];
    public IReadOnlyDictionary<string,IBootOperationChain> Chains => BootOperationsChains.AsReadOnly();
    
    private readonly List<string> _chainJoins = [];
    public IReadOnlyList<string> ChainJoins => _chainJoins.AsReadOnly();
    
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public bool TryRegister(string chainName, params Type[] types) => TryRegister<ChainVariables>(chainName, types);
    public bool TryRegister<T>(string chainName, params Type[] types) where T : IChainVariables {
        if (string.IsNullOrEmpty(chainName)) return false;
        if (types.Length == 0) return false;
        
        if (!BootOperationsChains.TryGetValue(chainName, out IBootOperationChain? chain)) {
            return BootOperationChain.TryCreate<T>(types, out chain) 
                && BootOperationsChains.TryAdd(chainName, chain);
        }

        foreach (Type type in types) {
            chain.BootOperations.AddLast(type);
        }
        
        _chainJoins.Add(chainName);
        return true;
    }
    
    public bool TryGetChain(string chainName, [NotNullWhen(true)] out IBootOperationChain? chain) => BootOperationsChains.TryGetValue(chainName, out chain);

    public bool TryAddToChain(string chainName, params Type[] types) {
        return !string.IsNullOrEmpty(chainName) 
            && types.Length != 0
            && BootOperationsChains.TryGetValue(chainName, out IBootOperationChain? chain)
            && chain.TryAddLastRange(types);
    }
    
    public BootOperationConfig Register(string chainName, params Type[] types) => Register<ChainVariables>(chainName, types);
    public BootOperationConfig Register<T>(string chainName, params Type[] types) where T : IChainVariables {
        if (!TryRegister<T>(chainName, types)) throw new InvalidOperationException($"Failed to register boot operations for chain '{chainName}'.");
        return this;
    }
    
    public IBootOperationChain GetChain(string chainName) {
        if (!TryGetChain(chainName, out IBootOperationChain? chain)) throw new InvalidOperationException($"Failed to get boot operations for chain '{chainName}'.");
        return chain;
    }

    public BootOperationConfig AddToChain(string chainName, params Type[] types) {
        if (!TryAddToChain(chainName, types)) throw new InvalidOperationException($"Failed to add boot operations to chain '{chainName}'.");
        return this;
    }
}
