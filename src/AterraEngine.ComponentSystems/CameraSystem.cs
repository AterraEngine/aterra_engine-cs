// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Numerics;
using AterraEngine.OldContracts.Assets;
using AterraEngine.OldContracts.Atlases;
using AterraEngine.OldContracts.Components;
using AterraEngine.OldContracts.Systems;
using AterraEngine.Types;

namespace AterraEngine.ComponentSystems;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class CameraSystem(IAssetAtlas assetAtlas) : ICameraSystem {
    public void Process(ICamera2D camera, float deltaTime) {
        if (!camera.TryGetComponent<ICamera2DComponent>(out var camera2DComponent)) return;
        
        // TODO fix this
        if (!assetAtlas.TryGetAsset(new EngineAssetId(new PluginId(0), 0), out var asset)) return;
        
        var player = (IPlayer2D)asset;

        if(!player.TryGetComponent<ITransform2DComponent>(out var playerTransform)) return;
        
        camera2DComponent.UpdateCamera(playerTransform.Pos, deltaTime);
        
    }
}