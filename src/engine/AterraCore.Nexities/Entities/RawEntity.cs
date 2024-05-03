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

public abstract class RawEntity<T>(params T[]? components) : AssetInstance, IRawEntity<T> where T : IComponent {
    protected readonly ConcurrentBag<T> ComponentsCache = new(components ?? []);
    public HashSet<Guid> ComponentIds => ComponentsCache
        .Select(comp => comp.Guid)
        .ToHashSet();

// -----------------------------------------------------------------------------------------------------------------
// Methods
// -----------------------------------------------------------------------------------------------------------------
    public IEnumerable<T> GetComponents() => ComponentsCache.AsEnumerable();
}
