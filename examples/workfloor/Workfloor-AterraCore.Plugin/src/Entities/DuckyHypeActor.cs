// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Common.Attributes;
using AterraCore.Contracts.Nexities.Components;
using AterraLib.Nexities.Entities;
using JetBrains.Annotations;
using Workfloor_AterraCore.Plugin.Components;

namespace Workfloor_AterraCore.Plugin.Entities;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[Entity(WorkfloorIdLib.Entities.ActorDuckyHype)]
[UsedImplicitly]
public class DuckyHypeActor(
    ITransform2D transform2D,
    [ResolveAsSpecific] SpriteDuckyHype sprite2D,
    IDirectChildren childEntities,
    IImpulse2D impulse2D
) : Actor2D(transform2D, sprite2D, childEntities, impulse2D);
