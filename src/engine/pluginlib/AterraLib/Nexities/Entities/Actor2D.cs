﻿// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Common.Attributes.Nexities;

namespace AterraLib.Nexities.Entities;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[Serializable]
[UsedImplicitly]
[Entity<IActor2D>(StringAssetIdLib.AterraLib.Entities.Actor2D)]
public class Actor2D(ITransform2D transform2D, ISprite2D sprite2D, IDirectChildren children, IImpulse2D impulse2D)
    : NexitiesEntity(transform2D, sprite2D, children, impulse2D), IActor2D {

    private IDirectChildren? _children = children;

    private IImpulse2D? _impulse2D = impulse2D;

    private ISprite2D? _sprite2D = sprite2D;
    private ITransform2D? _transform2D = transform2D;
    public ITransform2D Transform2D => _transform2D ??= GetComponent<ITransform2D>();
    public ISprite2D Sprite2D => _sprite2D ??= GetComponent<ISprite2D>();
    public IDirectChildren ChildrenIDs => _children ??= GetComponent<IDirectChildren>();
    public IImpulse2D Impulse2D => _impulse2D ??= GetComponent<IImpulse2D>();

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    protected override void ClearCaches() {
        _transform2D = null;
        _sprite2D = null;
        _children = null;
        _impulse2D = null;
    }
}
