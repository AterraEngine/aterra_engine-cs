// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Common.Attributes;
using AterraCore.Common.Types.Nexities;
using AterraLib.Nexities.Components;
using JetBrains.Annotations;
using Raylib_cs;
using Workfloor_AterraCore.Plugin.Contracts;

namespace Workfloor_AterraCore.Plugin.Components;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[Component<SpriteDuckyPlatinum>(WorkfloorIdLib.Components.SpriteDuckyPlatinum)]
[UsedImplicitly]
public class SpriteDuckyPlatinum : Sprite2D, ISpriteDuckyPlatinum {
    public override AssetId TextureAssetId { get; set; } = WorkfloorIdLib.Components.TextureDuckyPlatinum;
    public override Rectangle UvSelection { get; set; } = new(0, 0, 1, 1);
}
