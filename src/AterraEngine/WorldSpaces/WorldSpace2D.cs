// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Numerics;
using AterraEngine.Interfaces.Actors;
using AterraEngine.Interfaces.Atlases;
using AterraEngine.Interfaces.WorldSpaces;
using AterraEngine.Types;
using Raylib_cs;
using static Raylib_cs.Raymath;

namespace AterraEngine.WorldSpaces;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class WorldSpace2D : IWorldSpace2D {
    private Camera2D _camera;
    public Camera2D Camera {
        get => _camera;
        set {_camera = value; UpdateCamera(); }
    }

    public IPlayer2D Player2D { get; set; } = null!;
    public EngineAssetId Player2DId { get; set; }

    public Vector2 WorldToScreenSpace { get; private set; }
    public Vector2 ScreenToWorldSpace { get; private set; }

    public float DeltaTime { get; private set; }
    
    private void UpdateCamera() {
        WorldToScreenSpace = Raylib.GetWorldToScreen2D(Vector2.Zero, camera: _camera);
        ScreenToWorldSpace = Raylib.GetScreenToWorld2D(Vector2.Zero, camera: _camera);
    }
    
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public void RunSetup() {
        // Updating the camera, also update the World To Screen space
        _camera = new Camera2D {
            Target = Vector2.Zero,
            Offset = new Vector2 {
                X = Raylib.GetScreenWidth() / 2f,
                Y = Raylib.GetScreenHeight() / 2f
            },
            Rotation = 0.0f,
            Zoom = 0.1f
        };
        UpdateCamera();
        
        // THis should eventually be tied to the LEVEL
        IAssetAtlas assetAtlas = EngineServices.GetAssetAtlas();
        assetAtlas.TryGetAsset(Player2DId, out IPlayer2D? player2D);
        Player2D = player2D!;
        
        var textureAtlas = EngineServices.GetTextureAtlas();
        
        textureAtlas.TryLoadTexture(Player2D.Sprite.TextureId);
        textureAtlas.TryGetTexture(Player2D.Sprite.TextureId, out var spriteTexture);
        
        Player2D.Sprite.Texture = spriteTexture!;

        assetAtlas.TryGetAsset(new EngineAssetId(new PluginId(0), 16), out IAsset? asset);
        var level = (ILevel)asset!;
        
        level.Actors.ToList().ForEach(actor => {
            textureAtlas.TryLoadTexture(actor.Sprite.TextureId);
            textureAtlas.TryGetTexture(actor.Sprite.TextureId, out var texture2D);
            actor.Sprite.Texture = texture2D;
        });
        
    }
    
    public void UpdateFrame() {
        DeltaTime = Raylib.GetFrameTime();
        Player2D.LoadKeyMapping(DeltaTime);

        // if (Raylib.IsKeyDown(KeyboardKey.Two)) {
        //     _camera.Target.Y++;
        // }
        //
        UpdateCameraCenterSmoothFollow();
    }

    public void RenderFrameWorld() {
        Raylib.BeginMode2D(_camera);
        
        Player2D.Draw(WorldToScreenSpace);
        Player2D.DrawDebug(WorldToScreenSpace);
        
        IAssetAtlas assetAtlas = EngineServices.GetAssetAtlas();
        assetAtlas.TryGetAsset(new EngineAssetId(new PluginId(0), 16), out IAsset? asset);
        var level = (ILevel)asset!;
        level.Actors.ToList().ForEach(actor => actor.Draw(WorldToScreenSpace));

        Raylib.EndMode2D();
    }
    
    public void RenderFrameUi() {
        // Console.WriteLine(Camera.Target);
    }

    private void UpdateCameraCenterSmoothFollow() {
        const float lerpSpeed = 0.01f;
        const float minEffectLength = 500f;

        Vector2 playerScreenSpace = Player2D.Pos * WorldToScreenSpace; //TARGET IS IN SCREEN SPACE! so make the calculations!
        float length = Vector2Length(Vector2Subtract(playerScreenSpace, _camera.Target));
        
        if (!(length > minEffectLength)) return;
        
        // Smoothly interpolate the camera's target position.
        _camera.Target = Vector2.Lerp(_camera.Target, playerScreenSpace, DeltaTime) ; //TARGET IS IN SCREEN SPACE!
        
    }

}