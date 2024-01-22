// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Numerics;
using AterraEngine.Interfaces.WorldSpaces;
using Raylib_cs;

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

    public Vector2 WorldToScreenSpace { get; private set; }
    public Vector2 ScreenToWorldSpace { get; private set; }

    public float DeltaTime { get; private set; }


    private void UpdateCamera(Camera2D camera2D) {
        _camera = camera2D;
        WorldToScreenSpace = Raylib.GetWorldToScreen2D(Vector2.Zero, camera: Camera);
        ScreenToWorldSpace = Raylib.GetScreenToWorld2D(Vector2.Zero, camera: Camera);
    }
    
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public void RunSetup() {
        // Updating the camera, also update the World To Screen space
        Camera = new Camera2D(
            offset: new Vector2(Raylib.GetScreenWidth(),
                Raylib.GetScreenHeight()), // Camera offset (needs to be redefined on screen resize
            target: new Vector2(0, 0), // Camera target position
            rotation: 0.0f, // Camera rotation
            zoom: .1f // Camera zoom
        );
    }
    
    public void UpdateFrame() {
        DeltaTime = Raylib.GetFrameTime();
    }

    public void RenderFrameUi() {
        
    }

    public void RenderFrameWorld() {
        
    }
}