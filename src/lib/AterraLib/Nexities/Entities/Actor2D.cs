// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Contracts.Nexities.Data.Levels;

namespace AterraLib.Nexities.Entities;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[Serializable]
[UsedImplicitly]
[Entity<IActor2D>("AterraLib:Nexities/Entities/Actor2D")]
public class Actor2D(ITransform2D transform2D, ISprite2D sprite2D, IAssetTree childEntities) : NexitiesEntity(transform2D, sprite2D, childEntities), IActor2D {
    public ITransform2D Transform2D => (ITransform2D)Components.First(c => c.GetType().IsAssignableTo(typeof(ITransform2D)));
    public ISprite2D Sprite2D => (ISprite2D)Components.First(c => c.GetType().IsAssignableTo(typeof(ISprite2D)));
    public IAssetTree ChildEntities => (IAssetTree)Components.First(c => c.GetType().IsAssignableTo(typeof(IAssetTree)));
}
