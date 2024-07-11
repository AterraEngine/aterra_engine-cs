// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Contracts.Nexities.Data.Levels;
using Serilog;
using System.Collections.Immutable;

namespace AterraLib.Nexities.Systems;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[System("AterraLib:Nexities/Systems/Render2D", CoreTags.RenderSystem)]
public class Render2D(ILogger logger) : NexitiesSystem<IActor2D> {
    public ILogger Logger { get; } = logger.ForContext("Context", "Render2D");
    
    // -----------------------------------------------------------------------------------------------------------------
    // Helper Methods
    // -----------------------------------------------------------------------------------------------------------------
    private static IEnumerable<T> GetEntitiesInOrder<T>(T entity) where T : IHasAssetTree, IAssetInstance {
        foreach (T e in entity.ChildEntities.OfTypeManyReverse<T>()) {
            yield return e;        
        }
        yield return entity;
    }
    
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    protected override IEnumerable<IActor2D> EntitySelector(INexitiesLevel level) {
        return level.ChildEntities.OfTypeReverse<IActor2D>();
    }
    protected override void ProcessEntity(IActor2D originalEntity) {
        foreach (IActor2D childEntity in GetEntitiesInOrder(originalEntity)) {
            if (!childEntity.Sprite2D.TryGetTexture2D(out Texture2D? texture)) continue;
            
            Vector2 translation = childEntity.Transform2D.Translation + originalEntity.Transform2D.Translation;
            Vector2 scale = childEntity.Transform2D.Scale + originalEntity.Transform2D.Scale;
            Vector2 rotation = childEntity.Transform2D.Rotation + originalEntity.Transform2D.Rotation;
            
            Raylib.DrawTexturePro(
                texture: (Texture2D)texture, 
                source: childEntity.Sprite2D.TextureRectangle,
                dest: new Rectangle(translation, scale),
                origin: new Vector2(0,0), 
                rotation:rotation.X, 
                Color.White
            );
        }
    }
}
