// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

namespace AterraCore.Common;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[Flags]
public enum EngineConfigFlags : ulong{
    UnConfigured = 0ul,
    AssignedDefaultServices = 1ul << 0,
    AssignedStaticServices =  1ul << 1,
    ImportedEngineConfigDto = 1ul << 2,
    ImportedPlugins =         1ul << 3,
    ImportedPluginServices =  1ul << 4,
    DIContainerBuilt =        1ul << 5,
    
    
    // Configuration Issues?
    PluginLoadOrderUnstable = 1ul << 48,
}