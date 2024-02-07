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

public class Camera2DStaticComponent(IWorldSpace2D worldSpace2D) : Camera2DComponent(worldSpace2D), ICamera2DComponent {
    public override void UpdateCamera(Vector2 playerPos, float deltaTime) { }
}