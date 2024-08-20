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
public class Render2D(IAssetInstanceAtlas instanceAtlas, ILogger logger) : NexitiesSystemWithParentsReversed<IHasTransform2D,IActor2D> {
    private readonly Dictionary<AssetId, (Vector2 Size, Texture2D texture2D)> _texturesCache = new();
    
    // -----------------------------------------------------------------------------------------------------------------
    // Helper Methods
    // -----------------------------------------------------------------------------------------------------------------
    private (Vector2 Size, Texture2D texture2D) GetTextureAsset(AssetId textureAssetId) {
        if (_texturesCache.TryGetValue(textureAssetId, out (Vector2 Size, Texture2D texture2D) cacheHit)) return cacheHit;
        if (!instanceAtlas.TryGetOrCreateSingleton(textureAssetId, out ITexture2DAsset? textureAsset)) return (Vector2.One, new Texture2D());
        
        return _texturesCache[textureAssetId] = (textureAsset.Size, textureAsset.GetTexture());
    }
    
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public override void Tick(IActiveLevel level) {
        foreach ((IHasTransform2D? parent, IActor2D child) in GetEntities(level)) {
            (Vector2 textureSize, Texture2D texture2D) = GetTextureAsset(child.Sprite2D.TextureAssetId);
            
            if (parent is not null) ProcessChildEntities(parent, child, textureSize, texture2D);
            else ProcessOriginalEntity(child, textureSize, texture2D);
        }
    }
    
    private static void ProcessChildEntities(IHasTransform2D parent, IActor2D child, Vector2 textureSize, Texture2D texture2D) {
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

    private static void ProcessOriginalEntity(IActor2D originalEntity,  Vector2 textureSize, Texture2D texture2D) {
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
