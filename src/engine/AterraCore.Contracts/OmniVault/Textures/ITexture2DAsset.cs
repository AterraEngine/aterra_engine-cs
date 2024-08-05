// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Contracts.OmniVault.Assets;
using Raylib_cs;
using System.Numerics;

namespace AterraCore.Contracts.OmniVault.Textures;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface ITexture2DAsset : IAssetInstance {
    string imagePath { get; set; }
    Texture2D? Texture { get; set; }
    Vector2 Size { get; set; }
}
