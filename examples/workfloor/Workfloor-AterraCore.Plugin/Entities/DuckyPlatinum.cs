// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Attributes;
using AterraCore.Common.Types.Nexities;
using AterraCore.Contracts.Nexities.Components;
using AterraCore.Contracts.Nexities.Entities;
using AterraCore.Contracts.OmniVault.Textures;
using AterraCore.OmniVault.Textures;
using AterraLib.Nexities.Components;
using AterraLib.Nexities.Entities;
using JetBrains.Annotations;
using Raylib_cs;
using System.Numerics;

namespace Workfloor_AterraCore.Plugin.Entities;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface ITextureDuckyPlatinum : ITexture2DAsset;

public interface ISpriteDuckyPlatinum : ISprite2D;

public interface IDuckyPlatinumActor : IActor2D;

[Texture(WorkfloorIdLib.Components.TextureDuckyPlatinum)]
[UsedImplicitly]
public class TextureDuckyPlatinum : AbstractTexture2DAsset, ITextureDuckyPlatinum {
    public override string ImagePath { get; set; } = "resources/ducky-platinum.png";
    public override Vector2 Size { get; set; } = new(2048, 2048);
}

[Component<SpriteDuckyPlatinum>(WorkfloorIdLib.Components.SpriteDuckyPlatinum)]
[UsedImplicitly]
public class SpriteDuckyPlatinum : Sprite2D, ISpriteDuckyPlatinum {
    public override AssetId TextureAssetId { get; set; } = WorkfloorIdLib.Components.TextureDuckyPlatinum;
    public override Rectangle UvSelection { get; set; } = new(0, 0, 1, 1);
}

[Entity(WorkfloorIdLib.Entities.DuckyPlatinum)]
[UsedImplicitly]
public class DuckyPlatinumActor(
    ITransform2D transform2D,
    [InjectAs("01J5RA7EDMS1PRR1BMRN9XM9BU")] SpriteDuckyPlatinum sprite2D,
    IDirectChildren childEntities,
    IImpulse2D impulse2D
) : Actor2D(transform2D, sprite2D, childEntities, impulse2D), IDuckyPlatinumActor;
