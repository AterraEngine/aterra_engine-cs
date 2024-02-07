// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Numerics;
using OldAterraEngine.Contracts.Components;
using OldAterraEngine.Contracts.WorldSpaces;
using Raylib_cs;

namespace OldAterraEngine.Lib.Components;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public abstract class Camera2DComponent(IWorldSpace2D worldSpace2D) : ICamera2DComponent {
    protected Camera2D _camera = new Camera2D {
        Target = Vector2.Zero,
        Offset = new Vector2 {
            X = Raylib.GetScreenWidth() / 2f,
            Y = Raylib.GetScreenHeight() / 2f
        },
        Rotation = 0.0f,
        Zoom = 0.1f
    };
    public Camera2D Camera {
        get => _camera;
        set {_camera = value; UpdateCameraSpace(); }
    }
    
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public void UpdateCameraSpace() {
        worldSpace2D.WorldToScreenSpace = Raylib.GetWorldToScreen2D(Vector2.Zero, camera: _camera);
        worldSpace2D.ScreenToWorldSpace = Raylib.GetScreenToWorld2D(Vector2.Zero, camera: _camera);
    }

    public abstract void UpdateCamera(Vector2 playerPos, float deltaTime);
}