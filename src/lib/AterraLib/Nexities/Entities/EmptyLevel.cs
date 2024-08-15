// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace AterraLib.Nexities.Entities;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[Entity<INexitiesLevel>(AssetIdLib.AterraCore.Entities.EmptyLevel, CoreTags.Singleton)]
[UsedImplicitly]
public class EmptyLevel(IDirectChildren directChildren, ISystemIds systemIds) : NexitiesEntity(directChildren, systemIds), INexitiesLevel {
    public IDirectChildren ChildrenIDs { get; } = directChildren;
    public ISystemIds NexitiesSystemIds { get; } = systemIds;
}

