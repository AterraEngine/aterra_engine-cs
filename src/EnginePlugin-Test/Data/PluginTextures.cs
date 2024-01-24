// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Interfaces.Atlases;
using AterraEngine.Interfaces.Plugin;
using AterraEngine.Plugins;
using EnginePlugin_Test.Data.Textures;

namespace EnginePlugin_Test.Data;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class PluginTextures(ITexture2DAtlas texture2DAtlas) : AEnginePluginTextures(texture2DAtlas) {
    public override void Define() {
        Texture2DAtlas.TryRegisterTexture(TextureIds.DuckyTest);
    }
}