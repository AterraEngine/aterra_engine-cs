// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Common.Attributes.Nexities;
using AterraCore.OmniVault.Textures;
using JetBrains.Annotations;
using System.Numerics;
using Workfloor_AterraCore.Plugin.Contracts;

namespace Workfloor_AterraCore.Plugin.Components;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[Texture(WorkfloorIdLib.Components.TextureDuckyPlatinum)]
[UsedImplicitly]
public class TextureDuckyPlatinum : AbstractTexture2DAsset, ITextureDuckyPlatinum {
    public override string ImagePath { get; set; } = "resources/ducky-platinum.png";
    public override Vector2 Size { get; set; } = new(2048, 2048);
}
