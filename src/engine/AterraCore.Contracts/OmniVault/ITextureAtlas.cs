// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Common.Types.Nexities;

namespace AterraCore.Contracts.OmniVault;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface ITextureAtlas {
    public bool TryLoadAndRegisterTexture<T>(AssetId textureAssetId, string imagePath, out T? textureAsset, Guid? predefinedGuid = null)
        where T : class, ITexture2DAsset;
}
