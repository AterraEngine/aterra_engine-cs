// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Contracts.Assets;
using AterraEngine.Contracts.Atlases;
using AterraEngine.Contracts.Components;
using AterraEngine.Contracts.ECS;
using AterraEngine.Core.ECS.Render;
namespace AterraEngine.Lib.ComponentSystems.Render;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class Render2DSystem(ITexture2DAtlas texture2DAtlas) : RenderSystem<IActor>(texture2DAtlas) {
    public override Type[] ComponentTypes { get; } = [
        typeof(ITransform2DComponent),
        typeof(IDraw2DComponent)
    ];
    
    public override void Process(IEntity entity, float deltaTime, ICamera2DComponent camera2DComponent) {
        IActor actor = ConvertEntity(entity);
        
        // Apply the rotation to the movement
        actor.Drawable.Draw(
            actor.Transform.Pos,
            actor.Transform.Rot,
            actor.Transform.OriginRelative,
            actor.Transform.Size,
            camera2DComponent.WorldToScreenSpace
        );
        
        // if (!asset.TryGetComponent<IDrawDebug2DComponent>(out var drawDebug2D)) return;
        // drawDebug2D.Draw(
        //     transform2D.Pos,
        //     transform2D.Rot,
        //     transform2D.OriginRelative,
        //     transform2D.Size,
        //     camera2DComponent.WorldToScreenSpace
        // );
    }
}