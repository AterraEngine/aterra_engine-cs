// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.AssetVault.Textures;
using AterraCore.Common.Attributes.Nexities;
using JetBrains.Annotations;
using System.Numerics;
using Workfloor_AterraCore.Plugin.Contracts;

namespace Workfloor_AterraCore.Plugin.Components;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[Texture(WorkfloorIdLib.Components.TextureDuckyHype)]
[UsedImplicitly]
public class TextureDuckyHype : AbstractTexture2DAsset, ITextureDuckyHype {
    public override string ImagePath { get; set; } = "resources/ducky-hype.png";
    public override Vector2 Size { get; set; } = new(2048, 2048);
}
