// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

using System.Collections.Concurrent;
using AterraCore.Contracts.Nexities.Components;
using AterraCore.Contracts.Nexities.Entities;
using AterraCore.Nexities.Assets;
namespace AterraCore.Nexities.Entities;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public abstract class NexitiesEntity(params IComponent[]? components) : AssetInstance, INexitiesEntity {
    protected readonly ConcurrentBag<IComponent> ComponentsCache = new(components ?? []);
    public IEnumerable<IComponent> Components => ComponentsCache.AsEnumerable();
}