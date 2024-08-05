// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Contracts.Nexities.Data.Assets;
using Raylib_cs;
using System.Numerics;

namespace AterraCore.Contracts.OmniVault;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface ITexture2DAsset : IAssetInstance {
    string imagePath { get; set; }
    Texture2D? Texture { get; set; }
    Vector2 Size { get; set; }
}
