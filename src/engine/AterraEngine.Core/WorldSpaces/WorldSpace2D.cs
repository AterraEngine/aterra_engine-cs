// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Contracts.Assets;
using AterraEngine.Contracts.Atlases;
using AterraEngine.Contracts.Components;
using AterraEngine.Contracts.ECS;
using AterraEngine.Contracts.ECS.Camera;
using AterraEngine.Contracts.ECS.Logic;
using AterraEngine.Contracts.ECS.Render;
using AterraEngine.Contracts.ECS.Ui;
using AterraEngine.Contracts.WorldSpaces;
using AterraEngine.Core.ECS;
using AterraEngine.Core.Types;
using AterraEngine.Lib.ComponentSystems.Logic;
using AterraEngine.Lib.ComponentSystems.Render;
using Raylib_cs;

namespace AterraEngine.Core.WorldSpaces;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class WorldSpace2D : IWorldSpace2D {
    public float DeltaTime { get; private set; }
    
    public ILevel? LoadedLevel { get; set; }
    public EngineAssetId StartupLevelId { get; set; }
    
    private List<ILogicSystem<IAsset>> _logicSystems = [
        EngineServices.CreateWithServices<PlayerInput2DSystem>(),
        EngineServices.CreateWithServices<Transform2DSystem>()
    ];
    
    private List<IRenderSystem<IAsset>> _renderSystems = [
        EngineServices.CreateWithServices<Render2DSystem>()
    ];
    
    private List<IUiSystem<IEntity>> _uiSystems = [];
    private ICameraSystem<ICamera2D> _cameraSystem = EngineServices.CreateWithServices<Camera2DSystem>();
    private ICamera2DComponent? _camera2DComponent;

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public void RunSetup() {
        IAssetAtlas assetAtlas = EngineServices.GetAssetAtlas();
        assetAtlas.TryGetAsset(StartupLevelId, out ILevel? level);
        LoadedLevel = level!;

        if (!LoadedLevel.Camera2D.TryGetComponent(out ICamera2DComponent? camera2DComponent))
            throw new Exception("Camera Undefined");

        if (!assetAtlas.TryGetAsset(new EngineAssetId(new PluginId(0), 0), out IEngineAsset? player)) return;
        
        // Cache the camera for future use
        _camera2DComponent = camera2DComponent;
        _cameraSystem.Process(LoadedLevel.Camera2D, deltaTime: DeltaTime, (IAsset)player);

        foreach (var system in _renderSystems) {
            foreach (IAsset asset in LoadedLevel.Assets.Flat()) {
                system.LoadTextures(asset);
            }
        }
    }
    
    public void RunLogic() {
        
        IAssetAtlas assetAtlas = EngineServices.GetAssetAtlas();
        if (!assetAtlas.TryGetAsset(new EngineAssetId(new PluginId(0), 0), out IEngineAsset? player)) return;
        
        while (true) {
            DeltaTime = Raylib.GetFrameTime();
            LoadedLevel!.Assets.Flat();
        
            foreach (IAsset asset in LoadedLevel!.Assets.CachedFlat) {
                foreach (var system in _logicSystems) {
                    system.Process(asset, deltaTime: DeltaTime);
                }
            }
            _cameraSystem.Process(LoadedLevel.Camera2D, deltaTime: DeltaTime, (IAsset)player);
        }
    }

    public void RenderFrameWorld() {
        Raylib.ClearBackground(LoadedLevel!.BufferBackground);
        
        if (!LoadedLevel.Camera2D.TryGetComponent<ICamera2DComponent>(out var camera2DComponent)) throw new Exception("Camera Undefined");
        Raylib.BeginMode2D(camera2DComponent.Camera);
        
        foreach (IAsset asset in LoadedLevel!.Assets.CachedFlat) {
            foreach (var system in _renderSystems) {
                system.Process(asset, deltaTime:DeltaTime, camera2DComponent);
            }
        }

        Raylib.EndMode2D();
    }
    
    public void RenderFrameUi() {
        // Console.WriteLine(Camera.Target);

        var fps = Raylib.GetFPS();
        Raylib.DrawText($"{fps}", 20,20,20, Color.Black) ;
        
        // foreach (var system in _uiSystems) {
        //     foreach (IAsset asset in LoadedLevel!.Assets.CachedFlat) {
        //         
        //     }
        // }
        
    }
}