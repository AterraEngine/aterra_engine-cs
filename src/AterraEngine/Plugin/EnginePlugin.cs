// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine_lib.structs;
using AterraEngine.Interfaces.Plugin;
using Microsoft.Extensions.DependencyInjection;

namespace AterraEngine.Plugin;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class EnginePlugin : IEnginePlugin {
    public PluginId IdPrefix { get; protected set; }

    // -----------------------------------------------------------------------------------------------------------------
    // Methods for Plugin Manager to Use
    // -----------------------------------------------------------------------------------------------------------------
    public virtual IEnginePlugin DefineConfig(PluginId idPrefix) {
        IdPrefix = idPrefix;
        return this;
    }

    public virtual IServiceCollection DefineServices(IServiceCollection serviceCollection) {
        return serviceCollection;
    }

    public virtual IEnginePlugin DefineData() {
        return this;
    }
    
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
}