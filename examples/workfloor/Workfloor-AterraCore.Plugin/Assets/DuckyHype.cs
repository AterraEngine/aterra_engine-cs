// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Common.Types.Nexities;
using AterraCore.Contracts.Nexities.Data.Components;
using AterraCore.Contracts.Nexities.Data.Levels;
using AterraCore.Contracts.OmniVault;
using AterraCore.Contracts.OmniVault.Textures;
using AterraCore.Nexities.Components;
using AterraCore.Nexities.Entities;
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
public interface ITextureDuckyHype : ITexture2DAsset;
public interface ISpriteDuckyHype : ISprite2D;
public interface IDuckyHypeActor : IActor2D;

[Texture("Workfloor:TextureDuckyHype")]
[UsedImplicitly]
public class TextureDuckyHype : Texture2DAsset, ITextureDuckyHype {
    public override string imagePath { get; set; } = "assets/ducky-hype.png";
}

[Component("Workfloor:SpriteDuckyHype")]
[UsedImplicitly]
public class SpriteDuckyHype : Sprite2D, ISpriteDuckyHype {
    public override AssetId TextureAssetId { get; set; } = new("Workfloor:TextureDuckyHype");
    public override Rectangle UvSelection { get; set; } =  new(0, 0, 1, 1);
}

[Entity("Workfloor:ActorDuckyHype")]
[UsedImplicitly]
public class DuckyHypeActor(
    ITransform2D transform2D, 
    SpriteDuckyHype sprite2D,
    IAssetTree childEntities,
    IImpulse2D impulse2D
) : Actor2D(transform2D, sprite2D, childEntities,impulse2D), IDuckyHypeActor;