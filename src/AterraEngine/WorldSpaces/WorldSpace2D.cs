// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Numerics;
using AterraEngine.Contracts.Assets;
using AterraEngine.Contracts.Atlases;
using AterraEngine.Contracts.Systems;
using AterraEngine.Contracts.WorldSpaces;
using AterraEngine.ComponentSystems;
using AterraEngine.Contracts.Components;
using AterraEngine.Types;
using Raylib_cs;

namespace AterraEngine.WorldSpaces;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class WorldSpace2D : IWorldSpace2D {
    public float DeltaTime { get; private set; }
    
    public ILevel? LoadedLevel { get; set; }
    public EngineAssetId StartupLevelId { get; set; }
    
    private List<ILogicSystem> _logicSystems = [
        EngineServices.CreateWithServices<PlayerInput2DSystem>(),
        EngineServices.CreateWithServices<TransformSystem>()
    ];
    private List<IRenderSystem> _renderSystems = [
        EngineServices.CreateWithServices<Render2DSystem>()
    ];
    private List<IUiSystem> _uiSystems = [];
    private ICameraSystem _cameraSystem = EngineServices.CreateWithServices<CameraSystem>();
    
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public void RunSetup() {
        IAssetAtlas assetAtlas = EngineServices.GetAssetAtlas();
        assetAtlas.TryGetAsset(StartupLevelId, out ILevel? level);
        LoadedLevel = level!;
        
        // Updating the camera, also update the World To Screen space
        // Process the camera at least once
        _cameraSystem.Process(LoadedLevel.Camera2D, deltaTime:DeltaTime);
        
        foreach (var system in _renderSystems) {
            foreach (IAsset asset in LoadedLevel.Assets) {
                system.LoadTextures(asset);
            }
        }
    }
    
    public void UpdateFrame() {
        DeltaTime = Raylib.GetFrameTime();

        foreach (var system in _logicSystems) {
            foreach (IAsset asset in LoadedLevel!.Assets) {
                system.Process(asset, deltaTime:DeltaTime);
            }
        }
        // After everything is said and done with the assets, update the camera
        _cameraSystem.Process(LoadedLevel!.Camera2D, deltaTime:DeltaTime);

    }

    public void RenderFrameWorld() {
        Raylib.ClearBackground(LoadedLevel!.BufferBackground);
        
        if (!LoadedLevel.Camera2D.TryGetComponent<ICamera2DComponent>(out var camera2DComponent)) throw new Exception("Camera Undefined");
        Raylib.BeginMode2D(camera2DComponent.Camera);
        
        foreach (var system in _renderSystems) {
            foreach (IAsset asset in LoadedLevel.Assets) {
                system.Process(asset, deltaTime:DeltaTime, camera2DComponent);
            }
        }

        Raylib.EndMode2D();
    }
    
    public void RenderFrameUi() {
        // Console.WriteLine(Camera.Target);

        var fps = Raylib.GetFPS();
        Raylib.DrawText($"{fps}", 20,20,20, Color.Black) ;
        
        foreach (var system in from system in _uiSystems from asset in LoadedLevel.Assets select system) {
            // system.Process(asset, DeltaTime, WorldToScreenSpace);
        }
        
    }
}