// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Contracts.OmniVault.Textures;
using AterraCore.OmniVault.Assets;
using AterraCore.OmniVault.Textures;

namespace AterraLib.OmniVault.Textures;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[Texture("AterraLib:OmniVault/Textures/Texture2DAsset")]
public class Texture2DAsset : AssetInstance, ITexture2DAsset {
    public virtual string ImagePath { get; set; } = string.Empty;
    public Texture2D? Texture { get; set; }
    public Vector2 Size { get; set; } = Vector2.Zero;
}
