// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Numerics;
using AterraEngine.Contracts.Assets;
using AterraEngine.Contracts.Components;
using AterraEngine.Contracts.ECS.Logic;
using Raylib_cs;
namespace AterraEngine.Lib.ComponentSystems.Logic;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class Transform2DSystem : ILogicSystem<IAsset> {
    public Type[] ComponentTypes { get; } = [
        typeof(ITransform2DComponent),
        typeof(IMovement2DComponent)
    ];
    
    public void Process(IAsset asset, float deltaTime) {
        if(! asset.TryGetComponent(out ITransform2DComponent? position)) throw new Exception();
        if(! asset.TryGetComponent(out IMovement2DComponent? movement)) throw new Exception();
        
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