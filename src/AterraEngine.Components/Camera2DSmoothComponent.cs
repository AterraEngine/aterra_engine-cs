// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Numerics;
using AterraEngine.Contracts.Assets;
using AterraEngine.Contracts.Components;
using Raylib_cs;
using static Raylib_cs.Raymath;

namespace AterraEngine.Components;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------

public class Camera2DSmoothComponent : ICamera2DComponent {
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
    public Camera2DSmoothComponent() {
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
    
    public void UpdateCamera(Vector2 playerPos, float deltaTime) {
        const float lerpSpeed = 0.01f;
        const float minEffectLength = 500f;

        Vector2 playerScreenSpace = playerPos * WorldToScreenSpace; //TARGET IS IN SCREEN SPACE! so make the calculations!
        float length = Vector2Length(Vector2Subtract(playerScreenSpace, _camera.Target));
        
        if (!(length > minEffectLength)) return;
        
        // Smoothly interpolate the camera's target position.
        _camera.Target = Vector2.Lerp(_camera.Target, playerScreenSpace, deltaTime) ; //TARGET IS IN SCREEN SPACE!
    }
}