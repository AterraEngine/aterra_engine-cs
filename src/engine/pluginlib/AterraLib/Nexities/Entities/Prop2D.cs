// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace AterraLib.Nexities.Entities;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[Serializable]
[UsedImplicitly]
[Entity<IProp2D>(AssetIdStringLib.AterraLib.Entities.Prop2D)]
public class Prop2D(ITransform2D transform2D, ISprite2D sprite2D, IDirectChildren children)
    : NexitiesEntity(transform2D, sprite2D, children), IProp2D {
    private ITransform2D? _transform2D = transform2D;
    public ITransform2D Transform2D => _transform2D ??= GetComponent<ITransform2D>();

    private ISprite2D? _sprite2D = sprite2D;
    public ISprite2D Sprite2D => _sprite2D ??= GetComponent<ISprite2D>();

    private IDirectChildren? _children = children;
    public IDirectChildren ChildrenIDs => _children ??= GetComponent<IDirectChildren>();

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    protected override void ClearCaches() {
        _transform2D = null;
        _sprite2D = null;
        _children = null;
    }
}
