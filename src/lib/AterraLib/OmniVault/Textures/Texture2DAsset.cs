// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.OmniVault.Assets;
using AterraCore.OmniVault.Textures;

namespace AterraLib.OmniVault.Textures;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[Texture("AterraLib:OmniVault/Textures/Texture2DAsset")]
public class Texture2DAsset : AssetInstance, ITexture2DAsset {
    public virtual string imagePath { get; set; } = string.Empty;
    public Texture2D? Texture { get; set; }
}
