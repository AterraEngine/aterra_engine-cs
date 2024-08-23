// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.OmniVault.Textures;

namespace AterraLib.Nexities.Entities;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[Texture("AterraLib:Textures/AterraEngineLogo")]
[UsedImplicitly]
public class TextureAterraEngineLogo : AbstractTexture2DAsset {
    public override string ImagePath { get; set; } = "assets/aterra_engine-logo.png";
    public override Vector2 Size { get; set; } = new (1025, 1025);
}