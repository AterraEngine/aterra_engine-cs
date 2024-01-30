// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Numerics;
using AterraEngine.Contracts.Components;
using Raylib_cs;

namespace AterraEngine.Lib.Components;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------

public class Camera2DStaticComponent : ICamera2DComponent {
    private Camera2D _camera;
    public Camera2D Camera {
        get => _camera;
        set {_camera = value; UpdateCameraSpace(); }
    }

    public Vector2 WorldToScreenSpace {get; private set;}
    public Vector2 ScreenToWorldSpace {get; private set;}
    
    // -----------------------------------------------------------------------------------------------------------------
    // Constructors
    // -----------------------------------------------------------------------------------------------------------------
    public Camera2DStaticComponent() {
        _camera = new Camera2D {
            Target = Vector2.Zero,
            Offset = new Vector2 {
                X = Raylib.GetScreenWidth() / 2f,
                Y = Raylib.GetScreenHeight() / 2f
            },
            Rotation = 0.0f,
            Zoom = 0.1f
        };
        UpdateCameraSpace();
    }
    
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    private void UpdateCameraSpace() {
        WorldToScreenSpace = Raylib.GetWorldToScreen2D(Vector2.Zero, camera: _camera);
        ScreenToWorldSpace = Raylib.GetScreenToWorld2D(Vector2.Zero, camera: _camera);
    }
    
    public void UpdateCamera(Vector2 playerPos, float deltaTime) { }
}