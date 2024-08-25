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
    string ImagePath { get; set; }
    Vector2 Size { get; set; }

    bool TryGetTexture(out Texture2D texture);
    bool TrySetTexture(Texture2D texture);
    bool TryUnSetTexture(out Texture2D texture);

    Texture2D GetTexture();
}
