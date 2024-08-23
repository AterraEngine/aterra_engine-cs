// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Attributes;
using AterraCore.OmniVault.Textures;
using JetBrains.Annotations;
using System.Numerics;

namespace AterraLibOverwrite;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[Texture("AterraLibOverwrite:Textures/AterraEngineLogo")]
[OverridesAssetId("AterraLib:Textures/AterraEngineLogo")]
[UsedImplicitly]
public class TextureAterraEngineLogo : AbstractTexture2DAsset {
    public override string ImagePath { get; set; } = "assets/aterra_engine-logo-reverse.png";
    public override Vector2 Size { get; set; } = new (1025, 1025);
}