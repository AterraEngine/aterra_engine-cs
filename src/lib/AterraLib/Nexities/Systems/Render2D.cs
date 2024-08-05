// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Contracts.OmniVault.Assets;
using AterraCore.Contracts.OmniVault.Textures;

namespace AterraLib.Nexities.Systems;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[System("AterraLib:Nexities/Systems/Render2D", CoreTags.RenderSystem)]
public class Render2D(ITextureAtlas textureAtlas, IAssetInstanceAtlas instanceAtlas) : NexitiesSystem<IActor2D> {
    // -----------------------------------------------------------------------------------------------------------------
    // Helper Methods
    // -----------------------------------------------------------------------------------------------------------------
    private static IEnumerable<T> GetEntitiesInOrder<T>(T entity) where T : IHasAssetTree, IAssetInstance {
        return entity.AssetTree.OfTypeManyReverse<T>();
    }
    
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    protected override IEnumerable<IActor2D> EntitySelector(INexitiesLevel level) {
        return level.AssetTree.OfTypeReverse<IActor2D>();
    }
    protected override void ProcessEntity(IActor2D originalEntity) {
        foreach (IActor2D childEntity in GetEntitiesInOrder(originalEntity)) {
            if (!instanceAtlas.TryGetOrCreateSingleton(childEntity.Sprite2D.TextureAssetId, out ITexture2DAsset? texture2DAsset)) continue;
            if (texture2DAsset.Texture is null) continue;

            Vector2 translation = childEntity.Transform2D.Translation + originalEntity.Transform2D.Translation;
            Vector2 scale = childEntity.Transform2D.Scale * originalEntity.Transform2D.Scale;
            Vector2 rotation = childEntity.Transform2D.Rotation + originalEntity.Transform2D.Rotation;

            Raylib.DrawTexturePro(
                texture: (Texture2D)texture2DAsset.Texture,
                source: childEntity.Sprite2D.UvAndSourceCalculated ??= new Rectangle(childEntity.Sprite2D.UvSelection.Position, childEntity.Sprite2D.UvSelection.Size * texture2DAsset.Size ),
                dest: new Rectangle(translation, scale),
                origin: new Vector2(0, 0),
                rotation: rotation.X,
                Color.White
            );
        }
        if (!instanceAtlas.TryGetOrCreateSingleton(originalEntity.Sprite2D.TextureAssetId, out ITexture2DAsset? textureOriginalEntity)) return;
        if (textureOriginalEntity.Texture is null) return;
        Raylib.DrawTexturePro(
            texture: (Texture2D)textureOriginalEntity.Texture, 
            source: originalEntity.Sprite2D.UvAndSourceCalculated ??= new Rectangle(originalEntity.Sprite2D.UvSelection.Position, originalEntity.Sprite2D.UvSelection.Size * textureOriginalEntity.Size ),
            dest: originalEntity.Transform2D.DestinationRectangle,
            origin: new Vector2(0,0), 
            rotation:originalEntity.Transform2D.Rotation.X, 
            Color.White
        );
    }
}
