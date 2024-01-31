// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Numerics;
using AterraEngine.Contracts.Assets;
using AterraEngine.Contracts.Components;
using AterraEngine.Contracts.ECS;
using AterraEngine.Contracts.ECS.Logic;
using AterraEngine.Core.ECS.Logic;
using Raylib_cs;
namespace AterraEngine.Lib.ComponentSystems.Logic;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class Transform2DSystem : LogicSystem<IAsset> {
    public override Type[] ComponentTypes { get; } = [
        typeof(ITransform2DComponent),
        typeof(IMovement2DComponent)
    ];
    
    public override void Process(IEntity entity, float deltaTime) {
        IAsset asset = ConvertEntity(entity);
        
        asset.Transform.Size += asset.Movement.SizeOffset * deltaTime;
        asset.Transform.Rot += asset.Movement.RotationOffset * deltaTime;
        
        // Rotation has to be applied before position, because it relies on the rotation
        Vector2 newMovement = Vector2.Transform(
            asset.Movement.Direction, 
            Matrix3x2.CreateRotation(Raylib.DEG2RAD * (asset.Transform.Rot + 90))
            );
        
        asset.Transform.Pos += newMovement * asset.Movement.Speed * deltaTime;
    }
}