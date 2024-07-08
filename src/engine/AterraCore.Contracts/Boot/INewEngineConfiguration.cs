// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Common.Types.Nexities;

namespace AterraCore.Contracts.Boot;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface INewEngineConfiguration {
    INewEngineConfiguration RegisterBootOperation<T>() where T : IBootOperation, new();
    INewEngineConfiguration RegisterBootOperation(IBootOperation newOperation);
    INewEngineConfiguration RunBootOperations();
    IEngine BuildEngine();
}
