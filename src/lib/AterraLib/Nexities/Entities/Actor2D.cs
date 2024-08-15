// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace AterraLib.Nexities.Entities;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[Serializable]
[UsedImplicitly]
[Entity<IActor2D>(AssetIdLib.AterraCore.Entities.Actor2D)]
public class Actor2D(ITransform2D transform2D, ISprite2D sprite2D, IDirectChildren childEntities, IImpulse2D impulse2D) : NexitiesEntity(transform2D, sprite2D, childEntities, impulse2D), IActor2D {
    public ITransform2D Transform2D => transform2D;
    public ISprite2D Sprite2D => sprite2D;
    public IDirectChildren ChildrenIDs => childEntities;
    public IImpulse2D Impulse2D => impulse2D;
}
