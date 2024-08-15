// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Contracts.OmniVault.Assets;
using AterraCore.Contracts.OmniVault.Textures;
using AterraCore.Contracts.OmniVault.World;
using Serilog;
using Serilog.Core;

namespace AterraLib.Nexities.Systems.Rendering;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[System(AssetIdLib.AterraCore.SystemsRendering.Render2D, CoreTags.RenderSystem)]
[UsedImplicitly]
public class Render2D(IAssetInstanceAtlas instanceAtlas, ILogger logger) : NexitiesSystem<IActor2D>(entitiesReversed:true) {
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
        foreach (IActor2D entity in GetEntities(world)) {
            // ProcessChildEntities(entity);
            ProcessOriginalEntity(entity);
        }
    }
    
    // private void ProcessChildEntities(IActor2D originalEntity) {
    //     if (!instanceAtlas.TryGetOrCreateSingleton(childEntity.Sprite2D.TextureAssetId, out ITexture2DAsset? texture2DAsset) 
    //         || texture2DAsset.Texture is null) continue;
    //
    //     Vector2 translation = childEntity.Transform2D.Translation + originalEntity.Transform2D.Translation;
    //     Vector2 scale = childEntity.Transform2D.Scale * originalEntity.Transform2D.Scale;
    //     float rotation = childEntity.Transform2D.Rotation + originalEntity.Transform2D.Rotation;
    //     Vector2 rotationOrigin = childEntity.Transform2D.RotationOrigin + originalEntity.Transform2D.RotationOrigin;
    //
    //     DrawEntityTexture(
    //         (Texture2D)texture2DAsset.Texture, 
    //         childEntity.Sprite2D.UvAndSourceCalculated ??= new Rectangle(
    //             childEntity.Sprite2D.UvSelection.Position, 
    //             childEntity.Sprite2D.UvSelection.Size * texture2DAsset.Size
    //         ), 
    //         new Rectangle(translation, scale), 
    //         rotation,
    //         rotationOrigin
    //     );
    // }

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
