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
    ObjectPool<ConcurrentDictionary<AssetId, INexitiesComponent>> ComponentsByIdPool { get; }
    ObjectPool<ConcurrentDictionary<Type, AssetId>> ComponentsByTypePool { get; }
    ObjectPool<ConcurrentDictionary<Ulid, AssetId>> ComponentsByInstanceIdPool { get; }
}
