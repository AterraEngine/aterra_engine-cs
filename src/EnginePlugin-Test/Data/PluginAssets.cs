// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Numerics;
using AterraEngine;
using AterraEngine.Assets;
using AterraEngine.Contracts.Assets;
using AterraEngine.Contracts.Atlases;
using AterraEngine.Contracts.Components;
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
        var player2d = new Player2D(NewEngineAssetId(0), "player");
        if (!player2d.TryGetComponent<IDraw2DComponent>(out var draw2DComponent)) throw new Exception();

        draw2DComponent.TextureId = TextureIds.DuckyTest;
        draw2DComponent.SelectionBox = new Rectangle(0, 0, 1024, 1024);
            
        assetAtlas.TryRegisterAsset(player2d);

        List<IAssetNode> assets = [];

        // const int v = 10;
        //
        //
        // for (int i = -v; i < v; i++) {
        //     var duckyPrime = new Actor(NewEngineAssetId(), $"ducky{i:-2}A");
        //     
        //     if (!duckyPrime.TryGetComponent<IDraw2DComponent>(out var duckyDraw2DComponent)) throw new Exception();
        //     duckyDraw2DComponent.TextureId = TextureIds.DuckyTest;
        //     duckyDraw2DComponent.SelectionBox = new Rectangle(0, 0, 1024, 1024);
        //     
        //     if (!duckyPrime.TryGetComponent<ITransform2DComponent>(out var transform2DComponent)) throw new Exception();
        //     transform2DComponent.Pos = new Vector2(i, 25);
        //     
        //     assetAtlas.TryRegisterAsset(duckyPrime);
        //     
        //     List<IAssetNode> assetsNodes = [];
        //     
        //     for (int j = -v; j < v; j++) {
        //         var ducky = new Actor(NewEngineAssetId(), $"ducky{i:-2}{j:-2}");
        //     
        //         if (!ducky.TryGetComponent<IDraw2DComponent>(out var d)) throw new Exception();
        //         d.TextureId = TextureIds.DuckyTest;
        //         d.SelectionBox = new Rectangle(0, 0, 1024, 1024);
        //     
        //         if (!ducky.TryGetComponent<ITransform2DComponent>(out var t)) throw new Exception();
        //         t.Pos = new Vector2(i, 25);
        //     
        //         assetAtlas.TryRegisterAsset(ducky);
        //         assetsNodes.Add(new AssetNode(ducky));
        //         
        //     }
        //
        //     var currentNode = new AssetNode(duckyPrime, assetsNodes);
        //     assets.Add(currentNode);
        // }
        //
        var assetNode = new AssetNode(
            player2d,
            assets
        );
        
        EngineAssetId level1Id = NewEngineAssetId(9999);
        ILevel? level1 = levelFactory.CreateLevel(new LevelDto(level1Id, "LevelTest1"));
        
        EngineAssetId level2Id = NewEngineAssetId(99999);
        ILevel? level2 = levelFactory.CreateLevel(new LevelDto(level2Id, "LevelTest2", Color.Blue));
        level2.Assets.Children.Add(assetNode);
        level2.Assets.Asset = player2d;

        // level2.Camera2D = new Camera2DSmooth(NewEngineAssetId(), "camera");
        level2.Camera2D = new Camera2DStatic(NewEngineAssetId(), "camera");

        // worldSpace2D.StartupLevelId = level1Id;
        worldSpace2D.StartupLevelId = level2Id;
        
        
        Console.WriteLine(assetNode.Count());
        
    }
}