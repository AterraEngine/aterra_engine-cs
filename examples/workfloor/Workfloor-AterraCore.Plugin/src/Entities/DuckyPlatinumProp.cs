// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Common.Attributes;
using AterraCore.Common.Attributes.Nexities;
using AterraCore.Contracts.Nexities.Components;
using AterraLib.Nexities.Entities;
using JetBrains.Annotations;
using Workfloor_AterraCore.Plugin.Components;

namespace Workfloor_AterraCore.Plugin.Entities;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[Entity(WorkfloorIdLib.Entities.PropDuckyPlatinum)]
[UsedImplicitly]
public class DuckyPlatinumProp(
    ITransform2D transform2D,
    [ResolveAsSpecific] SpriteDuckyPlatinum sprite2D,
    IDirectChildren childEntities
) : Prop2D(transform2D, sprite2D, childEntities);
