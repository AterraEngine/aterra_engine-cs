// -----------------------------------------------------------------------------------------------------------------
// Methods
// -----------------------------------------------------------------------------------------------------------------
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
[Component<SpriteDuckyHype>(WorkfloorIdLib.Components.SpriteDuckyHype)]
[UsedImplicitly]
public class SpriteDuckyHype : Sprite2D, ISpriteDuckyHype {
    public override AssetId TextureAssetId { get; set; } = WorkfloorIdLib.Components.TextureDuckyHype;
    public override Rectangle UvSelection { get; set; } = new(0, 0, 1, 1);
}
