// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Numerics;
using AterraEngine.Contracts.Assets;
using AterraEngine.Contracts.Components;
using AterraEngine.Contracts.Systems;
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
        position.Rot += movement.RotationOffset * movement.Speed * deltaTime;
        
        // Rotation has to be applied before position, because it relies on the rotation
        var newMovement = Vector2.Transform(
            movement.Direction, 
            Matrix3x2.CreateRotation(Raylib.DEG2RAD * (position.Rot + 90))
            );
        
        position.Pos += newMovement* movement.Speed * deltaTime;

        
        Console.WriteLine(("Movement System",(movement.Direction, movement.RotationOffset), position.Pos, position.Rot));
    }
}