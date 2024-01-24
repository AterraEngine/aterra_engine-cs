// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Numerics;
using AterraEngine.Interfaces.Actors;
using AterraEngine.Interfaces.WorldSpaces;
using AterraEngine.Types;
using Raylib_cs;
using static Raylib_cs.Raymath;

namespace AterraEngine.WorldSpaces;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class WorldSpace2D : IWorldSpace2D{
    private Camera2D _camera;
    public Camera2D Camera {
        get => _camera;
        set => UpdateCamera(value);
    }

    public IPlayer2D Player2D { get; set; } = null!;
    public EngineAssetId Player2DId { get; set; }

    public Vector2 WorldToScreenSpace { get; private set; }
    public Vector2 ScreenToWorldSpace { get; private set; }

    public float DeltaTime { get; private set; }
    
    private void UpdateCamera(Camera2D camera2D) {
        _camera = camera2D;
        WorldToScreenSpace = Raylib.GetWorldToScreen2D(Vector2.Zero, camera: _camera);
        ScreenToWorldSpace = Raylib.GetScreenToWorld2D(Vector2.Zero, camera: _camera);
    }
    
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public void RunSetup() {
        // Updating the camera, also update the World To Screen space
        Camera = new Camera2D {
            Target = new Vector2(0, 0),
            Offset = new Vector2 {
                X = Raylib.GetScreenWidth() / 2f,
                Y = Raylib.GetScreenHeight() / 2f
            },
            Rotation = 0.0f,
            Zoom = .1f
        };
        
        // THis should eventually be tied to the LEVEL
        EngineServices.GetAssetAtlas().TryGetAsset(Player2DId, out IPlayer2D? player2D);
        Player2D = player2D!;
        
        var textureAtlas = EngineServices.GetTextureAtlas();
        
        Console.WriteLine(textureAtlas.TryLoadTexture(Player2D.Sprite.TextureId));
        Console.WriteLine(textureAtlas.TryGetTexture(Player2D.Sprite.TextureId, out var spriteTexture));
        
        Player2D.Sprite.Texture = spriteTexture!;
    }
    
    public void UpdateFrame() {
        DeltaTime = Raylib.GetFrameTime();
        Player2D.LoadKeyMapping(DeltaTime);
        
    }

    public void RenderFrameWorld() {
        UpdateCameraCenterSmoothFollow(ref _camera, Player2D, DeltaTime, Raylib.GetScreenWidth(), Raylib.GetScreenHeight());
        
        Player2D.Draw(WorldToScreenSpace);
        Player2D.DrawDebug(WorldToScreenSpace);
    }
    
    public void RenderFrameUi() {
        // Console.WriteLine(Camera.Target);
    }
    
    static void UpdateCameraCenterSmoothFollow(
        ref Camera2D camera,
        IActor player,
        float delta,
        int width,
        int height
    )
    {
        const float minSpeed = 30;
        const float minEffectLength = 10;
        const float fractionSpeed = 0.8f;

        camera.Offset = new Vector2(width / 2f, height / 2f);
        Vector2 diff = Vector2Subtract(player.Pos, camera.Target);
        float length = Vector2Length(diff);

        if (!(length > minEffectLength)) return;
        
        float speed = Math.Max(fractionSpeed * length, minSpeed);
        camera.Target = Vector2Add(camera.Target, Vector2Scale(diff, speed * delta / length));
    }

}