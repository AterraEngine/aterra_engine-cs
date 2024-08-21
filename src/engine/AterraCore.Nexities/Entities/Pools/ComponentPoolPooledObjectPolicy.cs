// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Common.Types.Nexities;
using AterraCore.Contracts.Nexities.Components;
using Microsoft.Extensions.ObjectPool;
using System.Collections.Concurrent;

namespace AterraCore.Nexities.Entities.Pools;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class ComponentPoolPooledObjectPolicy(int capacity) : PooledObjectPolicy<ConcurrentDictionary<AssetId, INexitiesComponent>> {
    public override ConcurrentDictionary<AssetId, INexitiesComponent> Create() => new(4, capacity);
    public override bool Return(ConcurrentDictionary<AssetId, INexitiesComponent> obj) {
        obj.Clear();
        return true;
    }
}
