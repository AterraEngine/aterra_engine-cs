// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Contracts.Assets;
using AterraEngine.Contracts.Atlases;
using AterraEngine.Contracts.Components;
using AterraEngine.Contracts.ECS.Camera;
using AterraEngine.Contracts.ECS.Logic;
using AterraEngine.Core.ECS.Ui;
using AterraEngine.Core.Types;
namespace AterraEngine.Lib.ComponentSystems.Logic;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class Camera2DSystem(IAssetAtlas assetAtlas) : CameraSystem<ICamera2D> {
    public override void Process(ICamera2D camera, float deltaTime, IAsset target) {
        // TODO fix this
        // if (!assetAtlas.TryGetAsset(new EngineAssetId(new PluginId(0), 0), out var asset)) return;
        
        IPlayer2D player = (IPlayer2D)target;

        if(!player.TryGetComponent(out ITransform2DComponent? playerTransform)) return;
        camera.Camera2DComponent.UpdateCamera(playerTransform.Pos, deltaTime);
        
    }
}