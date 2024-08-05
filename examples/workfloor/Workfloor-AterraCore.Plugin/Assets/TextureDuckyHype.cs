// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Common.Types.Nexities;
using AterraCore.Contracts.Nexities.Data.Components;
using AterraCore.Contracts.Nexities.Data.Levels;
using AterraCore.Contracts.OmniVault;
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

[Texture<ITextureDuckyHype>("Workfloor:TextureDuckyHype")]
[UsedImplicitly]
public class TextureDuckyHype : Texture2DAsset, ITextureDuckyHype {
    public override string imagePath { get; set; } = "assets/ducky-hype.png";
}

[Component<ISpriteDuckyHype>("Workfloor:SpriteDuckyHype")]
[UsedImplicitly]
public class SpriteDuckyHype : Sprite2D, ISpriteDuckyHype {
    public override AssetId TextureAssetId => "Workfloor:TextureDuckyHype";
    public override Rectangle Selection =>  new(0, 0, 2048, 2048);
}

[Entity<IDuckyHypeActor>("Workfloor:ActorDuckyHype")]
[UsedImplicitly]
public class DuckyHypeActor(
    ITransform2D transform2D, 
    ISpriteDuckyHype sprite2D,
    IAssetTree childEntities 
) : Actor2D(transform2D, sprite2D, childEntities), IDuckyHypeActor;