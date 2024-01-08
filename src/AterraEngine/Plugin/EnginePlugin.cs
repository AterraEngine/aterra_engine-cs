// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine_lib.structs;
using AterraEngine.Interfaces.Plugin;

namespace AterraEngine.Plugin;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class EnginePlugin:IEnginePlugin {
    public PluginId IdPrefix { get; private set; }
    public virtual void ManagedInitialize(PluginId idPrefix) {
        IdPrefix = idPrefix;
    }
}