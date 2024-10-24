// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace AterraEngine.Contracts.Builder.BootOperations;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IChainOperation<T> where T : IChainVariables {
    Task<T> ExecuteAsync(T chainVariables, CancellationToken cancellationToken);
}

public interface IChainOperation : IChainOperation<IChainVariables>;
