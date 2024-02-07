// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Numerics;
using AterraEngine.Contracts.Components;
using AterraEngine.Contracts.DTOs.ECS;
using AterraEngine.Contracts.ECS;
using AterraEngine.Contracts.ECS.EntityCombinations;
using AterraEngine.Contracts.WorldSpaces;
using AterraEngine.Core.ECS;
using Raylib_cs;
namespace AterraEngine.Lib.ComponentSystems.Render;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class Render2DSystem(IWorldSpace2D worldSpace2D) : EntityComponentSystem<IRender2DEntity> {
    public override void Update(IEntity e) {
        var entity = CastToEntity(e);
        
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
