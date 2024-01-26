// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Numerics;
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
        

        List<IAssetNode> assets = [];

        const int v = 500;
        

        for (int i = -v; i < v; i++) {
            var duckyPrime = new Actor(NewEngineAssetId(), $"ducky{i:-2}A");
            duckyPrime.Sprite = new Sprite {
                TextureId = TextureIds.DuckyTest,
                SelectionBox = new Rectangle(0,0, 1024, 1024)
            };
            duckyPrime.Pos = new Vector2(i, 25);
            assetAtlas.TryRegisterAsset(duckyPrime);
            
            List<IAssetNode> assetsNodes = [];
            
            for (int j = -v; j < v; j++) {
                var ducky = new Actor(NewEngineAssetId(), $"ducky{i:-2}{j:-2}");
                ducky.Sprite = new Sprite {
                    TextureId = TextureIds.DuckyTest,
                    SelectionBox = new Rectangle(0,0, 1024, 1024)
                };
                ducky.Pos = new Vector2(i, j);
                assetsNodes.Add(new AssetNode(ducky));
                assetAtlas.TryRegisterAsset(ducky);
            }

            var currentNode = new AssetNode(duckyPrime, assetsNodes);
            assets.Add(currentNode);
        }
        
        var ducky1 = new Actor(NewEngineAssetId(), "ducky{i:-2}{j:-2}");
        ducky1.Sprite = new Sprite {
            TextureId = TextureIds.DuckyTest,
            SelectionBox = new Rectangle(0,0, 1024, 1024)
        };

        var assetNode = new AssetNode(
            ducky1,
            assets
        );
        
        EngineAssetId level1Id = NewEngineAssetId();
        ILevel level1 = levelFactory.CreateLevel(new LevelDto(level1Id, "LevelTest1"));
        
        EngineAssetId level2Id = NewEngineAssetId();
        ILevel level2 = levelFactory.CreateLevel(new LevelDto(level2Id, "LevelTest2", Color.Blue));
        level2.Assets.Add(assetNode);
        level2.Assets.Add(new AssetNode(player2d));
        
        // worldSpace2D.StartupLevelId = level1Id;
        worldSpace2D.StartupLevelId = level2Id;
        
        
        Console.WriteLine(assetNode.Count());
        
    }
}