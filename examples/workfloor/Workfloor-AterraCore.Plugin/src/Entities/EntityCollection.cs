// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Common.Attributes.Nexities;
using AterraCore.Contracts.Nexities.Components;
using AterraCore.Contracts.Nexities.Entities.QuickHands;
using AterraCore.Nexities.Entities;
using JetBrains.Annotations;
using Workfloor_AterraCore.Plugin.Components;

namespace Workfloor_AterraCore.Plugin.Entities;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[Entity(WorkfloorIdLib.Entities.EntityCollection)]
[UsedImplicitly]
public class EntityCollection(IDirectChildren children, TagComponent tags) : NexitiesEntity(children, tags), IHasDirectChildren {
    private IDirectChildren? _children = children;
    public IDirectChildren ChildrenIDs => _children ??= GetComponent<IDirectChildren>();

    private TagComponent? _tags = tags;
    public TagComponent TagComponent => _tags ??= GetComponent<TagComponent>();

    public int Count => ChildrenIDs.Count;
}
