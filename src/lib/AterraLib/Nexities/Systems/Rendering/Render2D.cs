// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Contracts.Nexities.Entities.QuickHands;
using AterraCore.Contracts.OmniVault.Assets;
using AterraCore.Contracts.OmniVault.Textures;
using AterraCore.Contracts.OmniVault.World;
using Serilog;

namespace AterraLib.Nexities.Systems.Rendering;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[System(AssetIdLib.AterraCore.SystemsRendering.Render2D, CoreTags.RenderSystem)]
[UsedImplicitly]
public class Render2D(IAssetInstanceAtlas instanceAtlas) : NexitiesSystemWithParentsReversed<IHasTransform2D,IActor2D> {
    private readonly Dictionary<AssetId, (Vector2 Size, Texture2D texture2D)> _texturesCache = new();
    
    // -----------------------------------------------------------------------------------------------------------------
    // Helper Methods
    // -----------------------------------------------------------------------------------------------------------------
    private (Vector2 Size, Texture2D texture2D) GetTextureAsset(AssetId textureAssetId) {
        if (_texturesCache.TryGetValue(textureAssetId, out (Vector2 Size, Texture2D texture2D) cacheHit)) return cacheHit;
        if (!instanceAtlas.TryGetOrCreateSingleton(textureAssetId, out ITexture2DAsset? textureAsset)) return (Vector2.One, new Texture2D());
        
        return _texturesCache[textureAssetId] = (textureAsset.Size, textureAsset.GetTexture());
    }
    
    public override void InvalidateCaches() {
        base.InvalidateCaches();
        _texturesCache.Clear();
        _entityArray = null;
    }

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    private (IHasTransform2D? parent, IActor2D child)[]? _entityArray;
    public override void Tick(IActiveLevel level) {
        Span<(IHasTransform2D? parent, IActor2D child)> span = _entityArray ??= GetEntities(level).ToArray();
        
        foreach ((IHasTransform2D? parent, IActor2D child) in span) {
            if (parent is not null) ProcessChildEntities(parent, child);
            else ProcessOriginalEntity(child);
        }
    }
    
    private void ProcessChildEntities(IHasTransform2D parent, IActor2D child) {
        (Vector2 textureSize, Texture2D texture2D) = GetTextureAsset(child.Sprite2D.TextureAssetId);
        
        Raylib.DrawTexturePro(
            texture: texture2D, 
            source: child.Sprite2D.UvAndSourceCalculated ??= new Rectangle(
                child.Sprite2D.UvSelection.Position,
                child.Sprite2D.UvSelection.Size * textureSize
            ), 
            dest: new Rectangle(
                child.Transform2D.Translation + parent.Transform2D.Translation, 
                child.Transform2D.Scale * parent.Transform2D.Scale
            ), 
            origin: child.Transform2D.RotationOrigin + parent.Transform2D.RotationOrigin, 
            rotation: child.Transform2D.Rotation + parent.Transform2D.Rotation, 
            tint: Color.White
        );
    }

    private void ProcessOriginalEntity(IActor2D originalEntity) {
        (Vector2 textureSize, Texture2D texture2D) = GetTextureAsset(originalEntity.Sprite2D.TextureAssetId);
        
        Raylib.DrawTexturePro(
            texture: texture2D, 
            source: originalEntity.Sprite2D.UvAndSourceCalculated ??= new Rectangle(
                originalEntity.Sprite2D.UvSelection.Position,
                originalEntity.Sprite2D.UvSelection.Size * textureSize
            ), 
            dest: originalEntity.Transform2D.DestinationRectangle, 
            origin: originalEntity.Transform2D.RotationOrigin, 
            rotation: originalEntity.Transform2D.Rotation, 
            tint: Color.White
        );
    }
}
