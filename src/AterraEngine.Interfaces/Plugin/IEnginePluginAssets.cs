// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Interfaces.Atlases;
using AterraEngine.Types;

namespace AterraEngine.Interfaces.Plugin;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IEnginePluginAssets {
    public PluginId IdPrefix { get;}
    public IAssetAtlas AssetAtlas { get; }
    public void Define();

    public int NextInternalId();
    public EngineAssetId NewEngineAssetId();
    public EngineAssetId NewEngineAssetId(int value);
}