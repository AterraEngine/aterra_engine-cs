// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

using System.Numerics;
using AterraEngine.Contracts;
using AterraEngine.Frameworks.Nexities.Library.Components;
using AterraEngine.Frameworks.Nexities.Library.Entities;
using CodeOfChaos.Extensions.DependencyInjection;
using Raylib_cs;

namespace AterraEngine.Frameworks.Nexities.Library.Systems;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[InjectableSingleton<RenderEntitySystem>]
public class RenderEntitySystem(ITextureProvider textureProvider) : NexitiesSystem<IBasicEntity>{
    private readonly Rectangle _duckySource = new(0,0,256,256);
    
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public override void Update(in IBasicEntity entity, float delta) {
        if (!textureProvider.TryGetTexture(entity.Sprite.TextureId, out Texture2D texture)) return;
        (ITransformComponent transform, ISpriteComponent sprite) = entity;
            
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