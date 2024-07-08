// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Common.Types.Nexities;

namespace AterraCore.Contracts.Boot;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IEngineConfiguration {
    IEngineConfiguration RegisterBootOperation<T>() where T : IBootOperation, new();
    IEngineConfiguration RegisterBootOperation(IBootOperation newOperation);
    IEngineConfiguration RunBootOperations();
    IEngine BuildEngine();
}
