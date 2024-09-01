// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Common.Types.Nexities;

namespace AterraCore.Contracts.OmniVault.Textures;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface ITextureAtlas {
    public IEnumerable<ITexture2DAsset> TextureAssets { get; }

    #region Registering Texture to GPU
    bool TryRegisterTexture(AssetId textureAssetId);
    bool TryUnRegisterTexture(AssetId textureAssetId);
    #endregion
}
