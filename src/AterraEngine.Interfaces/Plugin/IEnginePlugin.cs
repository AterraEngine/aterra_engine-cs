// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Interfaces.Atlases;
using AterraEngine.Types;

namespace AterraEngine.Interfaces.Plugin;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IEnginePlugin {
    public PluginId IdPrefix { get;}
    public string NameReadable { get;}

    public IEnginePlugin DefineConfig(PluginId id);
    
    public IEnginePluginServices PluginServices();
    public IEnginePluginTextures PluginTextures(ITexture2DAtlas texture2DAtlas);
    public IEnginePluginAssets   PluginAssets(IAssetAtlas assetAtlas) ;
}