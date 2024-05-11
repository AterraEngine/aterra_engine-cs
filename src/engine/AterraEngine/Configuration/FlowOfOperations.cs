﻿// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

namespace AterraEngine.Configuration;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------

public enum FlowOfOperations : ulong {
    UnConfigured = 0ul,
    AssignedDefaultServices,
    AssignedStaticServices ,
    ImportedEngineConfigDto,
    ImportedPlugins,
    ImportedPluginServices,
    DiContainerBuilt,
}