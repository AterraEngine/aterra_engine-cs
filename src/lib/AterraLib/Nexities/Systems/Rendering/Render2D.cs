// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Contracts.OmniVault.Assets;
using AterraCore.Contracts.OmniVault.Textures;
using AterraCore.Contracts.OmniVault.World;

namespace AterraLib.Nexities.Systems.Rendering;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[System(AssetIdLib.AterraCore.SystemsRendering.Render2D, CoreTags.RenderSystem)]
[UsedImplicitly]
public class Render2D(IAssetInstanceAtlas instanceAtlas) : NexitiesSystemWithParents<IHasTransform2D,IActor2D> {
    protected override bool EntitiesReversed => true;
    
    // -----------------------------------------------------------------------------------------------------------------
    // Helper Methods
    // -----------------------------------------------------------------------------------------------------------------
    private static void DrawEntityTexture(Texture2D texture, Rectangle source, Rectangle dest, float rotation, Vector2 rotationOrigin) {
        Raylib.DrawTexturePro(texture, source, dest, rotationOrigin, rotation, Color.White);
    }
    
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public override void Tick(IAterraCoreWorld world) {
        foreach ((IActor2D? Parent, IActor2D Child) entity in GetEntities(world)) {
            if (entity.Parent is not null) ProcessChildEntities(entity.Parent, entity.Child);
            ProcessOriginalEntity(entity.Child);
        }
    }
    
    private void ProcessChildEntities(IActor2D parent, IActor2D child) {
        if (!instanceAtlas.TryGetOrCreateSingleton(child.Sprite2D.TextureAssetId, out ITexture2DAsset? texture2DAsset) 
            || texture2DAsset.Texture is null) return;
    
        Vector2 translation = child.Transform2D.Translation + parent.Transform2D.Translation;
        Vector2 scale = child.Transform2D.Scale * parent.Transform2D.Scale;
        float rotation = child.Transform2D.Rotation + parent.Transform2D.Rotation;
        Vector2 rotationOrigin = child.Transform2D.RotationOrigin + parent.Transform2D.RotationOrigin;
    
        DrawEntityTexture(
            (Texture2D)texture2DAsset.Texture, 
            child.Sprite2D.UvAndSourceCalculated ??= new Rectangle(
                child.Sprite2D.UvSelection.Position, 
                child.Sprite2D.UvSelection.Size * texture2DAsset.Size
            ), 
            new Rectangle(translation, scale), 
            rotation,
            rotationOrigin
        );
    }

    private void ProcessOriginalEntity(IActor2D originalEntity) {
        if (!instanceAtlas.TryGetOrCreateSingleton(originalEntity.Sprite2D.TextureAssetId, out ITexture2DAsset? texture2DAsset) 
            || texture2DAsset.Texture is null) return;

        Rectangle sourceRectangle = originalEntity.Sprite2D.UvAndSourceCalculated ??= new Rectangle(
            originalEntity.Sprite2D.UvSelection.Position, 
            originalEntity.Sprite2D.UvSelection.Size * texture2DAsset.Size
        );

        DrawEntityTexture((Texture2D)texture2DAsset.Texture, sourceRectangle, originalEntity.Transform2D.DestinationRectangle, originalEntity.Transform2D.Rotation, originalEntity.Transform2D.RotationOrigin);
    }
}
