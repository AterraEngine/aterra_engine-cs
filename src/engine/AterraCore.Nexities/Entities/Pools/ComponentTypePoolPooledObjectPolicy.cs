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
public class ComponentTypePoolPooledObjectPolicy(int capacity) : PooledObjectPolicy<ConcurrentDictionary<Type, AssetId>> {
    public override ConcurrentDictionary<Type, AssetId> Create() => new(4, capacity);
    public override bool Return(ConcurrentDictionary<Type, AssetId> obj) {
        obj.Clear();
        return true;
    }
}
