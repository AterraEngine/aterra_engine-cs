// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

using System.Numerics;
using Raylib_cs;

namespace AterraEngine.Frameworks.Nexities.Variants;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class RenderEntitySystem : NexitiesSystem<BasicEntity>{
    public Dictionary<string, Texture2D> TextureLookup { get; } = new();
    private readonly Rectangle _duckySource = new(0,0,256,256);
    
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public override void Update(in BasicEntity entity) {
        (TransformComponent transform, SpriteComponent sprite) = entity;
        Texture2D texture = TextureLookup[sprite.TextureId];
            
        Raylib.DrawTexturePro(
            texture,
            _duckySource,
            transform.Rectangle,
            Vector2.One,
            transform.Rotation,
            sprite.Tint
        );
    }
}