// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Common.Attributes;
using AterraCore.AssetVault.Textures;
using AterraCore.Common.Attributes.AssetVault;
using AterraCore.Common.Attributes.Nexities;
using AterraLib.Nexities.Components;

namespace AterraLib.Nexities.Entities;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[Texture("AterraLib:Textures/AterraEngineLogo")]
[UsedImplicitly]
public class TextureAterraEngineLogo : AbstractTexture2DAsset {
    public override string ImagePath { get; set; } = "assets/aterra_engine-logo.png";
    public override Vector2 Size { get; set; } = new(1025, 1025);
}

[Component<SpriteAterraEngineLogo>("AterraLib:Components/SpriteAterraEngineLogo")]
[UsedImplicitly]
public class SpriteAterraEngineLogo : Sprite2D {
    public override AssetId TextureAssetId { get; set; } = new("AterraLib:Textures/AterraEngineLogo");
    public override Rectangle UvSelection { get; set; } = new(0, 0, 1, 1);
}

[Component<TransFormAterraEngineLogo>("AterraLib:Components/TransformAterraEngineLogo", CoreTags.Singleton)]
[UsedImplicitly]
public class TransFormAterraEngineLogo : Transform2D {
    protected override Vector2 TranslationCache { get; set; } = new(0.5f, 0.5f);
    protected override Vector2 ScaleCache { get; set; } = new(50f, 50f);
}

[Entity<ActorAterraEngineLogo>("AterraLib:Actor/AterraEngineLogo", CoreTags.Singleton)]
[UsedImplicitly]
public class ActorAterraEngineLogo(
    [ResolveAsSpecific("01J7EEMGC6ZVS6ZW908A5ZTS6Y")] TransFormAterraEngineLogo transform2D,
    [ResolveAsSpecific("01J601YA03GS19CR8AHC9NCG55")] SpriteAterraEngineLogo sprite2D,
    IDirectChildren childEntities,
    IImpulse2D impulse2D
) : Actor2D(transform2D, sprite2D, childEntities, impulse2D);
