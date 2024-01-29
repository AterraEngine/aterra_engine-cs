// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.OldContracts.Atlases;
using AterraEngine.OldContracts.Factories;
using EnginePlugin_Test.Data.Textures;

namespace EnginePlugin_Test.Data;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class PluginTextures(ITexture2DAtlas texture2DAtlas) : APluginDataFactory {
    public override void CreateData() {
        texture2DAtlas.TryRegisterTexture(TextureIds.DuckyTest);
    }
}