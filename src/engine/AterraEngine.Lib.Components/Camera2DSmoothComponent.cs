// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Numerics;
using AterraEngine.Contracts.Components;
using AterraEngine.Contracts.WorldSpaces;
using Raylib_cs;

namespace AterraEngine.Lib.Components;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------

public class Camera2DSmoothComponent(IWorldSpace2D worldSpace2D) : Camera2DComponent(worldSpace2D), ICamera2DComponent {
    public override void UpdateCamera(Vector2 playerPos, float deltaTime) {
        const float lerpSpeed = 1f;
        const float minEffectLengthSquare = 500f * 500f;   // Compare the square of the lengths

        Vector2 playerScreenSpace = playerPos * worldSpace2D.WorldToScreenSpace;

        if (!(Vector2.Subtract(playerScreenSpace, _camera.Target).LengthSquared() > minEffectLengthSquare)) return;
    
        // Smoothly interpolate the camera's target position.
        _camera.Target = Vector2.Lerp(_camera.Target, playerScreenSpace, lerpSpeed*deltaTime); //TARGET IS IN SCREEN SPACE!
    }
}