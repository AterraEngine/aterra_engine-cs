// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Numerics;
using OldAterraEngine.Contracts.ECS;
using OldAterraEngine.Contracts.ECS.EntityCombinations;
using OldAterraEngine.Contracts.WorldSpaces;
using OldAterraEngine.Core.ECS;
using Raylib_cs;
namespace OldAterraEngine.Lib.ComponentSystems.Render;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class Render2DSystem(IWorldSpace2D worldSpace2D) : EntityComponentSystem<IRender2DEntity> {
    public override void Update(IEntity e) {
        IRender2DEntity? entity = CastToEntity(e);
        
        // Apply the rotation to the movement
        Vector2 adjustedPos = entity.Transform.Pos * worldSpace2D.WorldToScreenSpace;
        Vector2 adjustedSize = entity.Transform.Size * worldSpace2D.WorldToScreenSpace;
        Vector2 adjustedOrigin = entity.Transform.OriginRelative * worldSpace2D.WorldToScreenSpace;
        
        Raylib.DrawTexturePro(
            entity.Drawable.Texture ?? default,
            entity.Drawable.SelectionBox,
            new Rectangle(adjustedPos.X, adjustedPos.Y, adjustedSize.X, adjustedSize.Y),
            adjustedOrigin,
            entity.Transform.Rot,
            entity.Drawable.Tint
        );
    }
    
}
