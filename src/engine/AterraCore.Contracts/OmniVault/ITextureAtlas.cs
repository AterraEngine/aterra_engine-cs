// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Common.Types.Nexities;
using Raylib_cs;

namespace AterraCore.Contracts.OmniVault;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------

public interface ITextureAtlas {
    public bool TryLoadImage(AssetId assetId, string imagePath, out Texture2D? texture2D);
    public Texture2D LoadImage(string imagePath);
}
