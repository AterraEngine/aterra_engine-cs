// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace AterraLib.Nexities.Entities;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[Entity<INexitiesLevel2D>(AssetIdLib.AterraCore.Entities.EmptyLevel, CoreTags.Singleton)]
[UsedImplicitly]
public class EmptyLevel(IDirectChildren directChildren, ISystemIds systemIds) : NexitiesEntity(directChildren, systemIds), INexitiesLevel2D {
    public IDirectChildren ChildrenIDs { get; } = directChildren;
    public ISystemIds NexitiesSystemIds { get; } = systemIds;
}

