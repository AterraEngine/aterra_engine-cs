// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Contracts.Nexities.Entities.QuickHands;
using AterraCore.Contracts.OmniVault.Assets;
using AterraCore.Contracts.OmniVault.Textures;
using AterraCore.Contracts.OmniVault.World;
using AterraLib.Nexities.Components;
using Serilog;
using System.Collections.Immutable;

namespace AterraLib.Nexities.Systems.Rendering;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[System(AssetIdLib.AterraCore.SystemsRendering.Render2D, CoreTags.RenderSystem)]
[UsedImplicitly]
public class Render2D(IAssetInstanceAtlas instanceAtlas, ILogger logger) : NexitiesSystemWithParentsReversed<IHasTransform2D,IActor2D> {
    private ImmutableDictionary<AssetId, (Vector2 Size, Texture2D texture2D)> _texturesCache = ImmutableDictionary<AssetId, (Vector2 Size, Texture2D texture2D)>.Empty;
    
    // -----------------------------------------------------------------------------------------------------------------
    // Helper Methods
    // -----------------------------------------------------------------------------------------------------------------
    private (Vector2 Size, Texture2D texture2D) GetTextureAsset(AssetId textureAssetId) {
        if (_texturesCache.TryGetValue(textureAssetId, out (Vector2 Size, Texture2D texture2D) cacheHit)) return cacheHit;
        if (!instanceAtlas.TryGetOrCreateSingleton(textureAssetId, out ITexture2DAsset? textureAsset)) {
            logger.Error($"Asset {textureAssetId} not found");
            return (Vector2.One, new Texture2D());
        }

        (Vector2 Size, Texture2D) newValue = (textureAsset.Size, textureAsset.GetTexture());
        _texturesCache = _texturesCache.SetItem(textureAssetId, newValue);
        return newValue;
    }
    
    public override void InvalidateCaches() {
        base.InvalidateCaches();
        _texturesCache = _texturesCache.Clear();
    }

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    private static readonly Transform2D EmptyTransform2D = new();
    public override void Tick(ActiveLevel level) {
        foreach ((IHasTransform2D? parent, IActor2D? child) in GetEntities(level)) {
            ProcessChildEntities(
                parent?.Transform2D ?? EmptyTransform2D, 
                child
            );
        }
    }
    
    private void ProcessChildEntities(ITransform2D parentTransform, IActor2D child) {
        (Vector2 textureSize, Texture2D texture2D) = GetTextureAsset(child.Sprite2D.TextureAssetId);
        
        ITransform2D childTransform = child.Transform2D;
        Vector2 combinedTranslation = childTransform.Translation + parentTransform.Translation;
        var combinedScale = new Vector2(childTransform.Scale.X * parentTransform.Scale.X, childTransform.Scale.Y * parentTransform.Scale.Y);
        float combinedRotation = childTransform.Rotation + parentTransform.Rotation;
        Vector2 combinedRotationOrigin = childTransform.RotationOrigin + parentTransform.RotationOrigin;

        var destRect = new Rectangle(combinedTranslation, combinedScale.X, combinedScale.Y);

        Rectangle sourceRect = child.Sprite2D.UvAndSourceCalculated ??= new Rectangle(
            child.Sprite2D.UvSelection.Position,
            child.Sprite2D.UvSelection.Size * textureSize
        );
        
        Raylib.DrawTexturePro(
            texture: texture2D, 
            source: sourceRect, 
            dest: destRect, 
            origin: combinedRotationOrigin, 
            rotation: combinedRotation, 
            tint: Color.White
        );
    }
}
