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
[Entity(WorkfloorIdLib.Entities.PropDuckyHype)]
[UsedImplicitly]
public class DuckyHypeProp(
    ITransform2D transform2D,
    [ResolveAsSpecific] SpriteDuckyHype sprite2D,
    IDirectChildren childEntities
) : Prop2D(transform2D, sprite2D, childEntities);
