// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Interfaces.Atlases;
using AterraEngine.Interfaces.Plugin;
using AterraEngine.Types;
namespace AterraEngine.Plugins;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public abstract class AEnginePlugin : IEnginePlugin {
    public PluginId IdPrefix { get; private set; }
    public abstract string NameReadable { get; }
    
    public IEnginePlugin DefineConfig(PluginId id) {
        IdPrefix = id;
        return this;
    }
    
    public abstract IEnginePluginServices PluginServices();
    public abstract IEnginePluginTextures PluginTextures(ITexture2DAtlas texture2DAtlas);
    public abstract IEnginePluginAssets   PluginAssets(IAssetAtlas assetAtlas);
    
}