// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Numerics;
using AterraEngine.OldContracts.Assets;
using AterraEngine.OldContracts.Components;
using AterraEngine.OldContracts.Systems;
using Raylib_cs;

namespace AterraEngine.ComponentSystems;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class TransformSystem : ILogicSystem {
    public void Process(IAsset asset, float deltaTime) {
        if (!asset.TryGetComponent<ITransform2DComponent>(out var position)) return;
        if (!asset.TryGetComponent<IMovement2DComponent>(out var movement)) return;

        position.Size += movement.SizeOffset * deltaTime;
        position.Rot += movement.RotationOffset * deltaTime;
        
        // Rotation has to be applied before position, because it relies on the rotation
        var newMovement = Vector2.Transform(
            movement.Direction, 
            Matrix3x2.CreateRotation(Raylib.DEG2RAD * (position.Rot + 90))
            );
        
        position.Pos += newMovement* movement.Speed * deltaTime;
    }
}