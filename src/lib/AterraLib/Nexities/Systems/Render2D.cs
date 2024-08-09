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

    private static void DrawEntityTexture(Texture2D texture, Rectangle source, Rectangle dest, float rotation) {
        Raylib.DrawTexturePro(texture, source, dest, new Vector2(0, 0), rotation, Color.White);
    }
    
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    protected override IEnumerable<IActor2D> SelectEntities(INexitiesLevel? level) {
        return level.AssetTree.OfTypeReverse<IActor2D>();
    }
    
    protected override void ProcessEntity(IActor2D originalEntity) {
        ProcessChildEntities(originalEntity);
        ProcessOriginalEntity(originalEntity);
    }
    
    private void ProcessChildEntities(IActor2D originalEntity) {
        foreach (IActor2D childEntity in GetEntitiesInOrder(originalEntity)) {
            if (!instanceAtlas.TryGetOrCreateSingleton(childEntity.Sprite2D.TextureAssetId, out ITexture2DAsset? texture2DAsset) 
                || texture2DAsset.Texture is null) continue;

            Vector2 translation = childEntity.Transform2D.Translation + originalEntity.Transform2D.Translation;
            Vector2 scale = childEntity.Transform2D.Scale * originalEntity.Transform2D.Scale;
            Vector2 rotation = childEntity.Transform2D.Rotation + originalEntity.Transform2D.Rotation;

            DrawEntityTexture(
                (Texture2D)texture2DAsset.Texture, 
                childEntity.Sprite2D.UvAndSourceCalculated ??= new Rectangle(
                    childEntity.Sprite2D.UvSelection.Position, 
                    childEntity.Sprite2D.UvSelection.Size * texture2DAsset.Size
                ), 
                new Rectangle(translation, scale), 
                rotation.X
            );
        }
    }

    private void ProcessOriginalEntity(IActor2D originalEntity) {
        if (!instanceAtlas.TryGetOrCreateSingleton(originalEntity.Sprite2D.TextureAssetId, out ITexture2DAsset? texture2DAsset) 
            || texture2DAsset.Texture is null) return;

        Rectangle sourceRectangle = originalEntity.Sprite2D.UvAndSourceCalculated ??= new Rectangle(
            originalEntity.Sprite2D.UvSelection.Position, 
            originalEntity.Sprite2D.UvSelection.Size * texture2DAsset.Size
        );

        DrawEntityTexture((Texture2D)texture2DAsset.Texture, sourceRectangle, originalEntity.Transform2D.DestinationRectangle, originalEntity.Transform2D.Rotation.X);
    }
}
