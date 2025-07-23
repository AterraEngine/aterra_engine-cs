// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Numerics;
using AterraEngine.Contracts;
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
    public readonly Dictionary<uint, Texture2D> Textures = new();
    
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public override void Update(in IBasicEntity entity, float delta) {
       
        Raylib.DrawTexturePro(
            entity.Sprite.Texture2D,
            _duckySource,
            entity.Transform.Rectangle,
            Vector2.One,
            entity.Transform.Rotation,
            entity.Sprite.Tint
        );
    }
}