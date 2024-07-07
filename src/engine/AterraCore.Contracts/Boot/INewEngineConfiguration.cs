﻿// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Common.Types.Nexities;

namespace AterraCore.Contracts.Boot;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface INewEngineConfiguration {
    INewEngineConfiguration RegisterBootOperation(IBootOperation newOperation, AssetId? after = null);
    INewEngineConfiguration RunBootOperations();
    IEngine BuildEngine();
}
