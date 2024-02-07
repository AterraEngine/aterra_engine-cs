// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Numerics;
using OldAterraEngine.Contracts.Components;
using OldAterraEngine.Contracts.WorldSpaces;

namespace OldAterraEngine.Lib.Components;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------

public class Camera2DStaticComponent(IWorldSpace2D worldSpace2D) : Camera2DComponent(worldSpace2D), ICamera2DComponent {
    public override void UpdateCamera(Vector2 playerPos, float deltaTime) { }
}