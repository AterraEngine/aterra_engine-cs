// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

using AterraCore.Common.Nexities;
using AterraCore.Contracts.Nexities.Components.AssetTree;
using AterraCore.Nexities.Entities;
using JetBrains.Annotations;
using Nexities.Lib.Components.Sprite2D;
using Nexities.Lib.Components.Transform2D;

namespace Nexities.Lib.Entities.ActorEntity;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[UsedImplicitly]
[Entity<IActor2D>("AE000000")]
public class Actor2D(ITransform2D transform2D, ISprite2D sprite2D, IAssetTree childEntities) : NexitiesEntity(transform2D, sprite2D, childEntities), IActor2D {
    public ITransform2D Transform2D => transform2D;
    public ISprite2D Sprite2D => sprite2D;
    public IAssetTree ChildEntities => childEntities;
}

[UsedImplicitly]
[Entity<OakSprite2D>("AC000005", AssetInstanceType.Singleton)]
public class OakSprite2D : Sprite2D {
    public string Data = ""; // Whatever it is to override the sprite2d
}

public class TreeOak(ITransform2D transform2D, OakSprite2D oakSprite, IAssetTree childEntities) : Actor2D(transform2D, oakSprite, childEntities);
