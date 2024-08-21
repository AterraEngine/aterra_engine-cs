// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Common.Types.Nexities;
using AterraCore.Contracts.Nexities.Components;
using AterraCore.Contracts.Nexities.Entities;
using AterraCore.Contracts.OmniVault.Textures;
using AterraCore.Nexities.Components;
using AterraCore.Nexities.Entities;
using AterraCore.OmniVault.Assets.Attributes;
using AterraCore.OmniVault.Textures;
using AterraLib.Nexities.Components;
using AterraLib.Nexities.Entities;
using AterraLib.OmniVault.Textures;
using JetBrains.Annotations;
using Raylib_cs;
using System.Numerics;

namespace Workfloor_AterraCore.Plugin.Assets;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface ITextureDuckyHype : ITexture2DAsset;
public interface ISpriteDuckyHype : ISprite2D;
public interface IDuckyHypeActor : IActor2D;

[Texture("Workfloor:TextureDuckyHype")]
[UsedImplicitly]
public class TextureDuckyHype : Texture2DAsset, ITextureDuckyHype {
    public override string ImagePath { get; set; } = "assets/ducky-hype.png";
    public override Vector2 Size { get; set; } = new (2048, 2048);
}

[Component<SpriteDuckyHype>("Workfloor:SpriteDuckyHype")]
[UsedImplicitly]
public class SpriteDuckyHype : Sprite2D, ISpriteDuckyHype {
    public override AssetId TextureAssetId { get; set; } = new("Workfloor:TextureDuckyHype");
    public override Rectangle UvSelection { get; set; } =  new(0, 0, 1, 1);
}

[Entity("Workfloor:ActorDuckyHype")]
[UsedImplicitly]
public class DuckyHypeActor(
    ITransform2D transform2D, 
    [InjectAs("01J5RA7EDMS1PRR1BMRN9XM9BT")] SpriteDuckyHype sprite2D,
    IDirectChildren childEntities,
    IImpulse2D impulse2D
) : Actor2D(transform2D, sprite2D, childEntities,impulse2D), IDuckyHypeActor;