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
public abstract class EnginePlugin : IEnginePlugin {
    public PluginId IdPrefix { get; private set; }
    public abstract string NameInternal { get; }
    public abstract string NameReadable { get; }
    protected int _internalIdCounter;

    // -----------------------------------------------------------------------------------------------------------------
    // Methods for Plugin Manager to Use
    // -----------------------------------------------------------------------------------------------------------------
    public IEnginePlugin DefineConfig(PluginId idPrefix) {
        IdPrefix = idPrefix;
        return this;
    }

    public abstract void DefineServices(IServiceCollection serviceCollection);
    public abstract void DefineDataTextures();
    public abstract void DefineDataAssets();

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    
    // When the AssetAtlas is developed. Check if _internalIdCounter isn't already in there
    protected int NextInternalId() => _internalIdCounter++;
    protected EngineAssetId NewEngineAssetId() => new(IdPrefix, NextInternalId());
    protected EngineAssetId NewEngineAssetId(int value) => new(IdPrefix, value);
}