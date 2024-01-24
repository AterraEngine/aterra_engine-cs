// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Interfaces.Atlases;
using AterraEngine.Types;
using AterraEngine.Interfaces.Plugin;
using Microsoft.Extensions.DependencyInjection;

namespace AterraEngine.Plugins;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public abstract class AEnginePluginAssets(IAssetAtlas assetAtlas) : IEnginePluginAssets {
    public PluginId IdPrefix { get; private set; }
    public IAssetAtlas AssetAtlas { get; } = assetAtlas;
    
    protected int InternalIdCounter;
    
    // -----------------------------------------------------------------------------------------------------------------
    // Methods for Plugin Manager to Use
    // -----------------------------------------------------------------------------------------------------------------
    public IEnginePluginAssets DefineConfig(PluginId idPrefix) {
        IdPrefix = idPrefix;
        return this;
    }
    
    public virtual void Define() {}

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    // When the AssetAtlas is developed. Check if _internalIdCounter isn't already in there
    public int NextInternalId() => InternalIdCounter++;
    public EngineAssetId NewEngineAssetId() => new(IdPrefix, NextInternalId());
    public EngineAssetId NewEngineAssetId(int value) => new(IdPrefix, value);
}