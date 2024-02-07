// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Numerics;
using AterraEngine.Contracts.ECS;
using AterraEngine.Contracts.ECS.EntityCombinations;
using AterraEngine.Contracts.WorldSpaces;
using AterraEngine.Core.ECS;
using Raylib_cs;
namespace AterraEngine.Lib.ComponentSystems.Logic;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class Transform2DSystem(IWorldSpace2D worldSpace2D) : EntityComponentSystem<IMoveableEntity> {
    public override void Update(IEntity e) {
        var entity = CastToEntity(e);

        entity.Transform.Size += entity.Movement.SizeOffset * worldSpace2D.DeltaTime;
        entity.Transform.Rot += entity.Movement.RotationOffset * worldSpace2D.DeltaTime;
        
        // Rotation has to be applied before position, because it relies on the rotation
        Vector2 newMovement = Vector2.Transform(
            entity.Movement.Direction, 
            Matrix3x2.CreateRotation(Raylib.DEG2RAD * (entity.Transform.Rot + 90))
            );
        
        entity.Transform.Pos += newMovement * entity.Movement.Speed * worldSpace2D.DeltaTime;
        
    }
}