// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Contracts.Nexities.Entities.QuickHands;
using AterraCore.Contracts.OmniVault.Assets;
using AterraCore.Contracts.OmniVault.Textures;
using AterraCore.Contracts.OmniVault.World;
using AterraCore.Contracts.Threading.CrossThread;
using AterraLib.Nexities.Components;
using AterraLib.Nexities.Systems.CrossThreadDataHolders;

namespace AterraLib.Nexities.Systems.Rendering;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[System(AssetIdStringLib.AterraLib.SystemsRendering.Render2DPrep, CoreTags.RenderThread)]
[UsedImplicitly]
public class Render2DPrepForProps(IAssetInstanceAtlas instanceAtlas, ICrossThreadTickData crossThreadTickData) : NexitiesSystemWithParentsReversed<IHasTransform2D, IProp2D> {
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    private static readonly Transform2D EmptyTransform2D = new();
    public override void Tick(ActiveLevel level) {
        if (!crossThreadTickData.TryGetOrRegister(AssetTagLib.AterraLib.RenderableData, out RenderableData? renderableDataDto)) return;
        if (renderableDataDto.PropsProcessed) return;
        
        foreach ((IHasTransform2D? parent, IProp2D? child) in GetEntities(level).AsSpan()) {
            AssetId childTextureAssetId = child.Sprite2D.TextureAssetId;
            if (!renderableDataDto.TextureCache.TryGetValue(childTextureAssetId, out (Vector2 Size, Texture2D texture2D) tuple)) {
                if (!instanceAtlas.TryGetOrCreateSingleton(childTextureAssetId, out ITexture2DAsset? textureAsset)) continue;
                (Vector2 Size, Texture2D) newValue = (textureAsset.Size, textureAsset.GetTexture());
                renderableDataDto.TextureCache = renderableDataDto.TextureCache.SetItem(childTextureAssetId, newValue);
            }

            ITransform2D parentTransform = parent?.Transform2D ?? EmptyTransform2D;
            ITransform2D childTransform = child.Transform2D;
            Vector2 combinedTranslation = childTransform.Translation + parentTransform.Translation;
            var combinedScale = new Vector2(childTransform.Scale.X * parentTransform.Scale.X, childTransform.Scale.Y * parentTransform.Scale.Y);
            float combinedRotation = childTransform.Rotation + parentTransform.Rotation;
            Vector2 combinedRotationOrigin = childTransform.RotationOrigin + parentTransform.RotationOrigin;

            var destRect = new Rectangle(combinedTranslation, combinedScale.X, combinedScale.Y);

            Rectangle sourceRect = child.Sprite2D.UvAndSourceCalculated ??= new Rectangle(
                child.Sprite2D.UvSelection.Position,
                child.Sprite2D.UvSelection.Size * tuple.Size
            );
            
            renderableDataDto.RenderCache.Add((
                texture: tuple.texture2D, 
                source: sourceRect,
                dest : destRect,
                origin: combinedRotationOrigin,
                rotation: combinedRotation,
                tint: child.Sprite2D.Shade)
            );
            renderableDataDto.InstanceToIndexInRenderCache.Add(child.InstanceId, renderableDataDto.RenderCache.Count -1);
        }

        renderableDataDto.PropsProcessed = true;
    }
}
