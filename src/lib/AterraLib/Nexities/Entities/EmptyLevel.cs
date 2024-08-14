// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Contracts.Nexities.Components;
using AterraCore.Contracts.Nexities.Levels;
using AterraCore.Contracts.Nexities.Systems;

namespace AterraLib.Nexities.Entities;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[Entity<INexitiesLevel>("AterraLib:Nexities/Levels/Empty")]
[UsedImplicitly]
public class EmptyLevel(IDirectChildren directChildren, ISystemIds systemIds) : NexitiesEntity(directChildren, systemIds), INexitiesLevel {
    public IDirectChildren ChildrenIDs { get; } = directChildren;
    public ISystemIds NexitiesSystemIds { get; } = systemIds;
}

