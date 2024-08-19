// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Contracts.OmniVault.Assets;
using Raylib_cs;
using System.Diagnostics.CodeAnalysis;
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
}
