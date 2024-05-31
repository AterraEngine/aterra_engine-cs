// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace AterraCore.Common.Data;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------

public enum BootFlowOfOperations : ulong {
    UnConfigured = 0ul,
    AssignedFlexiPlugConfiguration,
    AssignedNexitiesConfiguration,
    
    ImportedEngineConfigDto,
    
    AssignedDefaultServices,
    AssignedStaticServices,
    
    DiContainerBuilt
}
