// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

using JetBrains.Annotations;

namespace AterraCore.Nexities.Lib.Entities.Actor;

using AterraCore.Contracts.Nexities.Data.Components.AssetTree;
using Components.Sprite2D;
using Components.Transform2D;
using Nexities.Entities;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[Serializable]
[UsedImplicitly]
[Entity<IActor2D>("AE000000")]
public class Actor2D(ITransform2D transform2D, ISprite2D sprite2D, IAssetTree childEntities) : NexitiesEntity(transform2D, sprite2D, childEntities), IActor2D {
    public ITransform2D Transform2D => transform2D;
    public ISprite2D Sprite2D => sprite2D;
    public IAssetTree ChildEntities => childEntities;
}