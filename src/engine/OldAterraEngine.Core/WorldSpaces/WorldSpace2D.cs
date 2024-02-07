// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Numerics;
using OldAterraEngine.Contracts.Assets;
using OldAterraEngine.Contracts.Atlases;
using OldAterraEngine.Contracts.Components;
using OldAterraEngine.Contracts.WorldSpaces;
using OldAterraEngine.Core.Types;
using Raylib_cs;

namespace OldAterraEngine.Core.WorldSpaces;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class WorldSpace2D : IWorldSpace2D {
    public float DeltaTime { get; private set; }
    public Vector2 WorldToScreenSpace { get; set;}
    public Vector2 ScreenToWorldSpace { get; set;}
    
    public ILevel? LoadedLevel { get; set; }
    public EngineAssetId StartupLevelId { get; set; }

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public void RunSetup() {
        IAssetAtlas assetAtlas = EngineServices.GetAssetAtlas();
        assetAtlas.TryGetAsset(StartupLevelId, out ILevel? level);
        LoadedLevel = level!;
        
        Console.WriteLine(level);

        foreach (var asset in LoadedLevel!.Assets.Flat()) {
            LoadTextures(asset);
        }

        LoadedLevel.GetCamera().GetRayLibCamera();
        LoadedLevel.GetCamera().Camera2DComponent.UpdateCameraSpace();
    }
    
    public void RunLogic(CancellationToken cancellationToken) {
        
        IAssetAtlas assetAtlas = EngineServices.GetAssetAtlas();
        if (!assetAtlas.TryGetAsset(new EngineAssetId(new PluginId(0), 0), out IEngineAsset? player)) return;
        
        while (!cancellationToken.IsCancellationRequested) {
            DeltaTime = Raylib.GetFrameTime();
            
            LoadedLevel!.LogicManager.Process(LoadedLevel!.Assets.Flat());
            
        }
    }

    public void RenderFrameWorld() {
        Raylib.ClearBackground(LoadedLevel!.BufferBackground);
        
        Raylib.BeginMode2D(LoadedLevel.GetCamera().GetRayLibCamera());
        
        LoadedLevel!.RenderManager.Process(LoadedLevel!.Assets.Flat());

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
    
    public void LoadTextures(IAsset? asset) {
        if (!asset.TryGetComponent<IDraw2DComponent>(out var draw2D)) return;
        
        ITexture2DAtlas texture2DAtlas = EngineServices.GetTexture2DAtlas();
        texture2DAtlas.TryLoadTexture(draw2D.TextureId);
        texture2DAtlas.TryGetTexture(draw2D.TextureId, out var texture2D);
        draw2D.Texture = texture2D;
    }

    public void UnloadTextures(IAsset asset) {
        if (!asset.TryGetComponent<IDraw2DComponent>(out var draw2D)) return;
        
        ITexture2DAtlas texture2DAtlas = EngineServices.GetTexture2DAtlas();
        texture2DAtlas.TryUnLoadTexture(draw2D.TextureId);
    }
}