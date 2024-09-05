﻿// ---------------------------------------------------------------------------------------------------------------------
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
public interface ITextureDuckyHype : ITexture2DAsset;

public interface ISpriteDuckyHype : ISprite2D;

public interface IDuckyHypeActor : IActor2D;

[Texture(WorkfloorIdLib.Components.TextureDuckyHype)]
[UsedImplicitly]
public class TextureDuckyHype : AbstractTexture2DAsset, ITextureDuckyHype {
    public override string ImagePath { get; set; } = "resources/ducky-hype.png";
    public override Vector2 Size { get; set; } = new(2048, 2048);
}

[Component<SpriteDuckyHype>(WorkfloorIdLib.Components.SpriteDuckyHype)]
[UsedImplicitly]
public class SpriteDuckyHype : Sprite2D, ISpriteDuckyHype {
    public override AssetId TextureAssetId { get; set; } = WorkfloorIdLib.Components.TextureDuckyHype;
    public override Rectangle UvSelection { get; set; } = new(0, 0, 1, 1);
}

[Entity(WorkfloorIdLib.Entities.DuckyHype)]
[UsedImplicitly]
public class DuckyHypeActor(
    ITransform2D transform2D,
    [InjectAs] SpriteDuckyHype sprite2D,
    IDirectChildren childEntities,
    IImpulse2D impulse2D
) : Actor2D(transform2D, sprite2D, childEntities, impulse2D), IDuckyHypeActor;
