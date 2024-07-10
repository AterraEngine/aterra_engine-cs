// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Contracts.Nexities.Data.Levels;
using Serilog;

namespace AterraLib.Nexities.Systems;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[System("AterraLib:Nexities/Systems/Render2D", CoreTags.RenderSystem)]
public class Render2D(ILogger logger) : NexitiesSystem<IActor2D> {
    public ILogger Logger { get; } = logger.ForContext("Context", "Render2D");

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public void ProcessSingularEntity(IActor2D entity) => ProcessEntity(entity);
    protected override void ProcessEntity(IActor2D entity) {
        IActor2D[] entities = [..entity.ChildEntities.OfTypeManyReverse<IActor2D>(), entity];
        
        foreach (IActor2D childEntity in entities) {
            if (!childEntity.Sprite2D.TryGetTexture2D(out Texture2D texture)) continue;
            Vector2 translation = childEntity.Transform2D.Translation + entity.Transform2D.Translation;
            Vector2 scale = childEntity.Transform2D.Scale + entity.Transform2D.Scale;
            Vector2 rotation = childEntity.Transform2D.Rotation + entity.Transform2D.Rotation;
            
            // Raylib.DrawTexture(texture, (int)translation.X, -(int)translation.Y, Color.White);
            
            Raylib.DrawTexturePro(
                texture: texture, 
                source: childEntity.Sprite2D.TextureRectangle,
                dest: new Rectangle(translation, scale),
                origin: new Vector2(0,0), 
                rotation:rotation.X, 
                Color.White
            );
        }
    }
}
