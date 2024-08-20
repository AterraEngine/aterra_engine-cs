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

namespace Workfloor_AterraCore.Plugin.Assets;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface ITextureDuckyPlatinum : ITexture2DAsset;
public interface ISpriteDuckyPlatinum : ISprite2D;
public interface IDuckyPlatinumActor : IActor2D;

[Texture("Workfloor:TextureDuckyPlatinum")]
[UsedImplicitly]
public class TextureDuckyPlatinum : Texture2DAsset,ITextureDuckyPlatinum {
    public override string ImagePath { get; set; } = "assets/ducky-platinum.png";
}

[Component<SpriteDuckyPlatinum>("Workfloor:SpriteDuckyPlatinum")]
[UsedImplicitly]
public class SpriteDuckyPlatinum : Sprite2D,ISpriteDuckyPlatinum  {
    public override AssetId TextureAssetId { get; set; } = new("Workfloor:TextureDuckyPlatinum");
    public override Rectangle UvSelection { get; set; } =  new(0, 0, 1, 1);
}

[Entity("Workfloor:ActorDuckyPlatinum")]
[UsedImplicitly]
public class DuckyPlatinumActor(
    ITransform2D transform2D, 
    [InjectAs("01J5RA7EDMS1PRR1BMRN9XM9BU")] SpriteDuckyPlatinum sprite2D,
    IDirectChildren childEntities,
    IImpulse2D impulse2D
) : Actor2D(transform2D, sprite2D, childEntities,impulse2D), IDuckyPlatinumActor;
