// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine;
using AterraEngine.Actors;
using AterraEngine.Interfaces.Atlases;
using AterraEngine.Interfaces.Plugin;
using AterraEngine.Plugins;
using AterraEngine.Types;
using EnginePlugin_Test.Data.Textures;
using Raylib_cs;

namespace EnginePlugin_Test.Data;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class PluginAssets(IAssetAtlas assetAtlas) : AEnginePluginAssets(assetAtlas) {
    public override void Define() {
        var worldSpace2D = EngineServices.GetWorldSpace2D();
        var player2d = new PlayerControllerLooking(NewEngineAssetId());
        worldSpace2D.Player2DId = player2d.Id;
        
        Sprite sprite = new Sprite {
            TextureId = TextureIds.DuckyTest,
            SelectionBox = new Rectangle(0,0, 1024, 1024)
        };

        player2d.Sprite = sprite;
        AssetAtlas.TryRegisterAsset(player2d);

        var ducky = new Actor(NewEngineAssetId(), "ducky1");
        ducky.Sprite = new Sprite {
            TextureId = TextureIds.DuckyTest,
            SelectionBox = new Rectangle(0,0, 1024, 1024)
        };

        var level = new Level {
            Id = NewEngineAssetId(16),
            InternalName = "LevelTest1",
            Actors = new[] {
                ducky
            }
        };
        AssetAtlas.TryRegisterAsset(level);
    }
}