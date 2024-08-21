// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Common.Types.Nexities;
using AterraCore.Contracts.Nexities.Components;
using Microsoft.Extensions.ObjectPool;
using System.Collections.Concurrent;

namespace AterraCore.Contracts.Nexities.Entities.Pools;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface INexitiesEntityPools {
    ObjectPool<ConcurrentDictionary<AssetId, INexitiesComponent>> ComponentPool { get; }
    ObjectPool<ConcurrentDictionary<Type, AssetId>> ComponentTypePool { get; }
}
