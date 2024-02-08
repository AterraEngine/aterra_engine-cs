// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Numerics;
using OldAterraEngine.Contracts.Assets;
using OldAterraEngine.Contracts.Atlases;
using OldAterraEngine.Contracts.Components;
using OldAterraEngine.Contracts.DTOs.Assets;
using OldAterraEngine.Contracts.Factories;
using OldAterraEngine.Core;
using OldAterraEngine.Core.Types;
using OldAterraEngine.Lib.Actors;
using OldAterraEngine.Lib.Actors.Entities;
using EnginePlugin_Test.Data.Textures;
using OldAterraEngine.Contracts.WorldSpaces;
using Raylib_cs;

namespace EnginePlugin_Test.Data;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class PluginAssets(IAssetAtlas assetAtlas, ILevelFactory levelFactory) : APluginDataFactory {
    public override void CreateData() {
        IWorldSpace2D? worldSpace2D = EngineServices.GetWorldSpace2D();
        var player2d = new Player2D(NewEngineAssetId(0), "player");
        var camera = new Camera2DSmooth(NewEngineAssetId(1));
        
        if (!player2d.TryGetComponent<IDraw2DComponent>(out IDraw2DComponent? draw2DComponent)) throw new Exception();

        draw2DComponent.TextureId = TextureIds.DuckyTest;
        draw2DComponent.SelectionBox = new Rectangle(0, 0, 1024, 1024);
            
        assetAtlas.TryRegisterAsset(player2d);
        assetAtlas.TryRegisterAsset(camera);

        List<IAssetNode> assets = [
            new AssetNode(player2d),
            new AssetNode(camera)
        ];

        const int v = 50;
        
        
        for (int i = -v; i < v; i++) {
            var duckyPrime = new Actor(NewEngineAssetId(), $"ducky{i:-2}A") {
                Drawable = {
                    TextureId = TextureIds.DuckyTest,
                    SelectionBox = new Rectangle(0, 0, 1024, 1024)
                },
                Transform = {
                    Pos = new Vector2(i, 25)
                }
            };
        
            assetAtlas.TryRegisterAsset(duckyPrime);
            
            List<IAssetNode> assetsNodes = [];
            
            for (int j = -v; j < v; j++) {
                var ducky = new Actor(NewEngineAssetId(), $"ducky{i:-2}{j:-2}") {
                    Drawable = {
                        TextureId = TextureIds.DuckyTest,
                        SelectionBox = new Rectangle(0, 0, 1024, 1024)
                    },
                    Transform = {
                        Pos = new Vector2(i, j)
                    }
                };
        
                assetAtlas.TryRegisterAsset(ducky);
                assetsNodes.Add(new AssetNode(ducky));
            }
        
            var currentNode = new AssetNode(duckyPrime, assetsNodes);
            assets.Add(currentNode);
        }
        
        EngineAssetId level1Id = NewEngineAssetId();
        ILevel? level1 = levelFactory.CreateLevel(new LevelDto(level1Id, "LevelTest1"));
        
        EngineAssetId level2Id = NewEngineAssetId();
        ILevel? level2 = levelFactory.CreateLevel(new LevelDto(level2Id, "LevelTest2", Color.Blue));
        level2.Assets = new AssetNodeRoot(
            assets
        );
        assetAtlas.TryRegisterAsset(level2);
        
        // level2.Assets.Asset = player2d;

        // level2.Camera2D = ;
        // level2.Camera2D = new Camera2DStatic(NewEngineAssetId(), "camera");

        // worldSpace2D.StartupLevelId = level1Id;
        worldSpace2D.StartupLevelId = level2Id;
        
        Console.WriteLine(level2.Assets.Count());
        
    }
}