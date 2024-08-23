// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Contracts.Nexities.Entities.QuickHands;
using AterraCore.Contracts.OmniVault.Assets;
using AterraCore.Contracts.OmniVault.Textures;
using AterraCore.Contracts.OmniVault.World;
using AterraLib.Nexities.Components;
using System.Collections.Immutable;

namespace AterraLib.Nexities.Systems.Rendering;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[System(AssetIdLib.AterraCore.SystemsRendering.Render2D, CoreTags.RenderSystem)]
[UsedImplicitly]
public partial class Render2D(IAssetInstanceAtlas instanceAtlas) : NexitiesSystemWithParentsReversed<IHasTransform2D,IActor2D> {
    private ImmutableDictionary<AssetId, (Vector2 Size, Texture2D texture2D)> _texturesCache = ImmutableDictionary<AssetId, (Vector2 Size, Texture2D texture2D)>.Empty;
    
    // -----------------------------------------------------------------------------------------------------------------
    // Helper Methods
    // -----------------------------------------------------------------------------------------------------------------
    private (Vector2 Size, Texture2D texture2D) GetTextureAsset(AssetId textureAssetId) {
        if (_texturesCache.TryGetValue(textureAssetId, out (Vector2 Size, Texture2D texture2D) cacheHit)) return cacheHit;
        if (!instanceAtlas.TryGetOrCreateSingleton(textureAssetId, out ITexture2DAsset? textureAsset)) return (Vector2.One, new Texture2D());

        (Vector2 Size, Texture2D) newValue = (textureAsset.Size, textureAsset.GetTexture());
        _texturesCache = _texturesCache.SetItem(textureAssetId, newValue);
        return newValue;
    }
    
    public override void InvalidateCaches() {
        base.InvalidateCaches();
        _texturesCache = _texturesCache.Clear();
    }
}
