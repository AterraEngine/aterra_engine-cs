// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

namespace AterraCore.Nexities.Data.Entities;

using Assets;
using Contracts.Nexities.Data.Components;
using Contracts.Nexities.Data.Entities;
using System.Collections.Concurrent;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public abstract class NexitiesEntity(params IComponent[]? components) : AssetInstance, INexitiesEntity {
    protected readonly ConcurrentBag<IComponent> ComponentsCache = new(components ?? []);
    public IEnumerable<IComponent> Components => ComponentsCache.AsEnumerable();
}