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
public interface ITextureDuckyPlatinum : ITexture2DAsset;
public interface ISpriteDuckyPlatinum : ISprite2D;
public interface IDuckyPlatinumActor : IActor2D;

[Texture<ITextureDuckyPlatinum>("Workfloor:TextureDuckyPlatinum")]
[UsedImplicitly]
public class TextureDuckyPlatinum : Texture2DAsset,ITextureDuckyPlatinum {
    public override string imagePath { get; set; } = "assets/ducky-platinum.png";
}

[Component<ISpriteDuckyPlatinum>("Workfloor:SpriteDuckyPlatinum")]
[UsedImplicitly]
public class SpriteDuckyPlatinum : Sprite2D,ISpriteDuckyPlatinum  {
    public override AssetId TextureAssetId => "Workfloor:TextureDuckyPlatinum";
    public override Rectangle Selection =>  new(0, 0, 2048, 2048);
}

[Entity<IDuckyPlatinumActor>("Workfloor:ActorDuckyPlatinum")]
[UsedImplicitly]
public class DuckyPlatinumActor(
    ITransform2D transform2D, 
    ISpriteDuckyPlatinum sprite2D,
    IAssetTree childEntities 
) : Actor2D(transform2D, sprite2D, childEntities), IDuckyPlatinumActor;
