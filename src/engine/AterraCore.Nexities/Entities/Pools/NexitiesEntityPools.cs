// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Common.Types.Nexities;
using AterraCore.Contracts.Nexities.Components;
using AterraCore.Contracts.Nexities.Entities.Pools;
using JetBrains.Annotations;
using Microsoft.Extensions.ObjectPool;
using System.Collections.Concurrent;

namespace AterraCore.Nexities.Entities.Pools;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[UsedImplicitly]
public class NexitiesEntityPools : INexitiesEntityPools {
    private const int InitialCapacity = 12; // Very small basic capacity, because entities don't hold that many components
    private readonly DefaultObjectPoolProvider _objectPoolProvider = new();

    private ObjectPool<ConcurrentDictionary<AssetId, INexitiesComponent>>? _componentPool;
    public ObjectPool<ConcurrentDictionary<AssetId, INexitiesComponent>> ComponentPool => 
        _componentPool ??= _objectPoolProvider.Create(new ComponentPoolPooledObjectPolicy(InitialCapacity));

    private ObjectPool<ConcurrentDictionary<Type, AssetId>>? _componentTypePool;
    public ObjectPool<ConcurrentDictionary<Type, AssetId>> ComponentTypePool => 
        _componentTypePool ??= _objectPoolProvider.Create(new ComponentTypePoolPooledObjectPolicy(InitialCapacity));
}
