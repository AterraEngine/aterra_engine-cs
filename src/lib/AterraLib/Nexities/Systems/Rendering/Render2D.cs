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
    private readonly Dictionary<AssetId, ITexture2DAsset> _texturesCache = new();
    private (IHasTransform2D? Parent, IActor2D Child)[]? _entityArray;
    
    // -----------------------------------------------------------------------------------------------------------------
    // Helper Methods
    // -----------------------------------------------------------------------------------------------------------------
    private ITexture2DAsset? GetTextureAsset(AssetId textureAssetId) {
        if (_texturesCache.TryGetValue(textureAssetId, out ITexture2DAsset? textureAsset)) return textureAsset;
        if (instanceAtlas.TryGetOrCreateSingleton(textureAssetId, out textureAsset)) _texturesCache[textureAssetId] = textureAsset;
        return textureAsset;
    }
    
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public override void Tick(IAterraCoreWorld world) {
        (IHasTransform2D? Parent, IActor2D Child)[] entities =_entityArray ??= GetEntities(world).ToArray();

        foreach ((IHasTransform2D? Parent, IActor2D Child) entity in entities.AsSpan()) {
            if (entity.Parent is not null) {
                ProcessChildEntities(entity.Parent, entity.Child);
            }
            ProcessOriginalEntity(entity.Child);
        }
    }
    
    private void ProcessChildEntities(IHasTransform2D parent, IActor2D child) {
        ITexture2DAsset? textureAsset = GetTextureAsset(child.Sprite2D.TextureAssetId);
        if (textureAsset is null || !textureAsset.TryGetTexture(out Texture2D texture)) return;
        
        Rectangle sourceRectangle = child.Sprite2D.UvAndSourceCalculated ??= new Rectangle(
            child.Sprite2D.UvSelection.Position,
            child.Sprite2D.UvSelection.Size * textureAsset.Size
        );

        var destRectangle = new Rectangle(
            child.Transform2D.Translation + parent.Transform2D.Translation, 
            child.Transform2D.Scale * parent.Transform2D.Scale
        );

        Raylib.DrawTexturePro(
            texture: texture, 
            source: sourceRectangle, 
            dest: destRectangle, 
            origin: child.Transform2D.RotationOrigin + parent.Transform2D.RotationOrigin, 
            rotation: child.Transform2D.Rotation + parent.Transform2D.Rotation, 
            tint: Color.White
        );
    }

    private void ProcessOriginalEntity(IActor2D originalEntity) {
        ITexture2DAsset? textureAsset = GetTextureAsset(originalEntity.Sprite2D.TextureAssetId);
        if (textureAsset is null || !textureAsset.TryGetTexture(out Texture2D texture)) return;

        Rectangle sourceRectangle = originalEntity.Sprite2D.UvAndSourceCalculated ??= new Rectangle(
            originalEntity.Sprite2D.UvSelection.Position,
            originalEntity.Sprite2D.UvSelection.Size * textureAsset.Size
        );
        
        Raylib.DrawTexturePro(
            texture: texture, 
            source: sourceRectangle, 
            dest: originalEntity.Transform2D.DestinationRectangle, 
            origin: originalEntity.Transform2D.RotationOrigin, 
            rotation: originalEntity.Transform2D.Rotation, 
            tint: Color.White
        );
    }
}
