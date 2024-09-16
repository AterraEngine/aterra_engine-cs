// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Common.Attributes.Nexities;
using AterraCore.Contracts.Nexities.Entities.QuickHands;
using AterraCore.Contracts.Nexities.Systems;
using AterraCore.Contracts.OmniVault.Assets;
using AterraCore.Contracts.OmniVault.Textures;
using AterraCore.Contracts.OmniVault.World;
using AterraCore.Contracts.Threading.CrossThread;
using AterraLib.Nexities.Components;
using AterraLib.Nexities.DataHolders;

namespace AterraLib.Nexities.Systems.Rendering;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[UsedImplicitly]
[System(StringAssetIdLib.AterraLib.SystemsRendering.Render2DPrepForActors, CoreTags.LogicThread)]
public class Render2DPrepForActors(IAssetInstanceAtlas instanceAtlas, ICrossThreadTickData crossThreadTickData) : NexitiesSystemWithParentsReversed<IHasTransform2D, IActor2D>, ILogicSystem {

    private static readonly Transform2D EmptyTransform2D = new();
    // -----------------------------------------------------------------------------------------------------------------
    // Helper Methods
    // -----------------------------------------------------------------------------------------------------------------
    private (Vector2 Size, Texture2D texture2D) GetTextureAsset(AssetId textureAssetId, RenderableData renderableData) {
        if (renderableData.TextureCache.TryGetValue(textureAssetId, out (Vector2 Size, Texture2D texture2D) cacheHit)) return cacheHit;
        if (!instanceAtlas.TryGetOrCreateSingleton(textureAssetId, out ITexture2DAsset? textureAsset)) return (Vector2.One, new Texture2D());

        (Vector2 Size, Texture2D) newValue = (textureAsset.Size, textureAsset.GetTexture());
        renderableData.TextureCache.TryAdd(textureAssetId, newValue);
        return newValue;
    }

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public void LogicTick(IActiveLevel level) {
        if (!crossThreadTickData.TryGetOrRegister(AssetIdLib.AterraLib.TickDataHolders.RenderableData, out RenderableData? renderableDataDto)) return;

        foreach ((IHasTransform2D? parent, IActor2D child, int zIndex) in GetEntities(level).AsSpan()) {
            (Vector2 size, Texture2D texture2D) = GetTextureAsset(child.Sprite2D.TextureAssetId, renderableDataDto);

            ITransform2D parentTransform = parent?.Transform2D ?? EmptyTransform2D;
            ITransform2D childTransform = child.Transform2D;

            renderableDataDto.AddToRenderCache(
                zIndex,
                new RenderCacheDto {
                    Texture = texture2D,
                    Source = child.Sprite2D.UvAndSourceCalculated ??= new Rectangle(
                        child.Sprite2D.UvSelection.Position,
                        child.Sprite2D.UvSelection.Size * size
                    ),
                    Dest = new Rectangle(
                        childTransform.Translation + parentTransform.Translation,
                        childTransform.Scale.X * parentTransform.Scale.X,
                        childTransform.Scale.Y * parentTransform.Scale.Y),
                    Origin = childTransform.RotationOrigin + parentTransform.RotationOrigin,
                    Rotation = childTransform.Rotation + parentTransform.Rotation,
                    Tint = child.Sprite2D.Shade
                }
            );
        }
    }
}
