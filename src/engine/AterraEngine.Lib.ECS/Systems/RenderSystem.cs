// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Numerics;
using AterraEngine.Contracts.Core;
using AterraEngine.Contracts.Lib.ECS.ComponentComposites;
using AterraEngine.Core.ECSFramework;
using Raylib_cs;
namespace AterraEngine.Lib.ECS.Systems;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
// TODO add DI WorldSpace
public class RenderSystem(IWorldSpace2D worldSpace2D) : System<IDrawable2D> {
    public override void Process(IDrawable2D entity) {
        
        // Apply the rotation to the movement
        Vector2 adjustedPos = entity.Transform2D.Position * worldSpace2D.WorldToScreenSpace;
        Vector2 adjustedSize = entity.Transform2D.Scale * worldSpace2D.WorldToScreenSpace;
        Vector2 adjustedOrigin = entity.Sprite.OriginRelative * worldSpace2D.WorldToScreenSpace;
        
        Raylib.DrawTexturePro(
            entity.Sprite.Texture ?? default,
            entity.Sprite.SelectionBox,
            new Rectangle(adjustedPos.X, adjustedPos.Y, adjustedSize.X, adjustedSize.Y),
            adjustedOrigin,
            entity.Transform2D.Rotation,
            entity.Sprite.Tint
        );
    }
}