// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Diagnostics.CodeAnalysis;

namespace AterraEngine.Contracts.Builder;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IBootOperationBuilder {
    bool TryRegister(string chainName, params Type[] types);
    bool TryGetChain(string chainName, [NotNullWhen(true)] out IBootOperationChain? chain);
    bool TryAddToChain(string chainName, params Type[] types);
    
    IBootOperationBuilder Register(string chainName, params Type[] types);
    IBootOperationBuilder AddToChain(string chainName, params Type[] types);
    IBootOperationChain GetChain(string chainName);
}
