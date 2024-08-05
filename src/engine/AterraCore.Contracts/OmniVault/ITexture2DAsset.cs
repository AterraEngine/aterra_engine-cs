// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Common.Types.Nexities;
using AterraCore.Contracts.Nexities.Data.Assets;
using Raylib_cs;

namespace AterraCore.Contracts.OmniVault;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface ITexture2DAsset : IAssetInstance {
    string imagePath { get; set; }
    Texture2D? Texture { get; set; }
}
