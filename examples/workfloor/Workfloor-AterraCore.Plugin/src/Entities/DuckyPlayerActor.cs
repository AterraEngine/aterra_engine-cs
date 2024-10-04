// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Common.Attributes;
using AterraCore.Common.Attributes.Nexities;
using AterraCore.Contracts.Nexities.Components;
using AterraCore.Nexities.Entities;
using JetBrains.Annotations;
using Workfloor_AterraCore.Plugin.Components;
using Workfloor_AterraCore.Plugin.Contracts;

namespace Workfloor_AterraCore.Plugin.Entities;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[Entity(WorkfloorIdLib.Entities.DuckyPlayer)]
[UsedImplicitly]
public class DuckyPlayerActor(
    ITransform2D transform2D,
    [ResolveAsSpecific] SpriteDuckyPlatinum sprite2D,
    IDirectChildren childEntities,
    IImpulse2D impulse2D,
    BoundingCircle boundingCircle
) : NexitiesEntity(transform2D, sprite2D, childEntities, impulse2D, boundingCircle), 
    IDuckyPlayerActor, IHasBoundingCircle {

    private BoundingCircle? _boundingCircle = boundingCircle;
    public BoundingCircle BoundingCircle => _boundingCircle ??= GetComponent<BoundingCircle>();

    private ITransform2D? _transform2D = transform2D;
    public ITransform2D Transform2D => _transform2D ??= GetComponent<ITransform2D>();

    private ISprite2D? _sprite2D = sprite2D;
    public ISprite2D Sprite2D => _sprite2D ??= GetComponent<ISprite2D>();

    private IDirectChildren? _childrenIDs = childEntities;
    public IDirectChildren ChildrenIDs => _childrenIDs ??= GetComponent<IDirectChildren>();

    private IImpulse2D? _impulse2D = impulse2D;
    public IImpulse2D Impulse2D => _impulse2D ??= GetComponent<IImpulse2D>(); 
}
