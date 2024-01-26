// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine;
using AterraEngine.Assets;
using AterraEngine.Contracts.Assets;
using AterraEngine.Contracts.Atlases;
using AterraEngine.Contracts.Factories;
using AterraEngine.DTO.Assets;
using AterraEngine.Types;
using EnginePlugin_Test.Data.Textures;
using Raylib_cs;

namespace EnginePlugin_Test.Data;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class PluginAssets(IAssetAtlas assetAtlas, ILevelFactory levelFactory) : APluginDataFactory {
    public override void CreateData() {
        var worldSpace2D = EngineServices.GetWorldSpace2D();
        var player2d = new PlayerControllerLooking(NewEngineAssetId());
        worldSpace2D.Player2DId = player2d.Id;
        
        Sprite sprite = new Sprite {
            TextureId = TextureIds.DuckyTest,
            SelectionBox = new Rectangle(0,0, 1024, 1024)
        };

        player2d.Sprite = sprite;
        assetAtlas.TryRegisterAsset(player2d);

        var ducky = new Actor(NewEngineAssetId(), "ducky1");
        ducky.Sprite = new Sprite {
            TextureId = TextureIds.DuckyTest,
            SelectionBox = new Rectangle(0,0, 1024, 1024)
        };
        
        EngineAssetId level1Id = NewEngineAssetId(16);
        ILevel level1 = levelFactory.CreateLevel(new LevelDto(level1Id, "LevelTest1"));
        
        EngineAssetId level2Id = NewEngineAssetId(32);
        ILevel level2 = levelFactory.CreateLevel(new LevelDto(level2Id, "LevelTest2", Color.Beige));
        level2.Actors.Add(ducky);
        
        // worldSpace2D.StartupLevelId = level1Id;
        worldSpace2D.StartupLevelId = level2Id;
    }
}