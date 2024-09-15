// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Common.Attributes;
using AterraCore.Common.Types.Nexities;
using AterraCore.Contracts.Nexities.Components;
using AterraCore.Contracts.PoolCorps;
using JetBrains.Annotations;
using Microsoft.Extensions.ObjectPool;
using System.Collections.Concurrent;

namespace AterraCore.PoolCorps;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[UsedImplicitly]
[Singleton<NexitiesEntityPools, INexitiesEntityPools>]
public class NexitiesEntityPools : INexitiesEntityPools {
    // Small initial capacity as entities usually don't hold many components.
    private const int InitialCapacity = 12;
    private readonly DefaultObjectPoolProvider _objectPoolProvider = new();

    #region ComponentsByIdPool
    private ObjectPool<ConcurrentDictionary<AssetId, INexitiesComponent>>? _componentsByIdPool;
    public ObjectPool<ConcurrentDictionary<AssetId, INexitiesComponent>> ComponentsByIdPool =>
        _componentsByIdPool ??= _objectPoolProvider.Create(new ComponentsByIdPoolPolicy(InitialCapacity));

    private class ComponentsByIdPoolPolicy(int capacity) : PooledObjectPolicy<ConcurrentDictionary<AssetId, INexitiesComponent>> {
        public override ConcurrentDictionary<AssetId, INexitiesComponent> Create() => new(4, capacity);
        public override bool Return(ConcurrentDictionary<AssetId, INexitiesComponent> obj) {
            obj.Clear();
            return true;
        }
    }
    #endregion
    #region ComponentsByTypePool
    private ObjectPool<ConcurrentDictionary<Type, AssetId>>? _componentsByTypePool;
    public ObjectPool<ConcurrentDictionary<Type, AssetId>> ComponentsByTypePool =>
        _componentsByTypePool ??= _objectPoolProvider.Create(new ComponentsByTypePoolPolicy(InitialCapacity));

    private class ComponentsByTypePoolPolicy(int capacity) : PooledObjectPolicy<ConcurrentDictionary<Type, AssetId>> {
        public override ConcurrentDictionary<Type, AssetId> Create() => new(4, capacity);
        public override bool Return(ConcurrentDictionary<Type, AssetId> obj) {
            obj.Clear();
            return true;
        }
    }
    #endregion
    #region ComponentsByInstanceIdPool
    private ObjectPool<ConcurrentDictionary<Ulid, AssetId>>? _componentsByInstanceIdPool;
    public ObjectPool<ConcurrentDictionary<Ulid, AssetId>> ComponentsByInstanceIdPool =>
        _componentsByInstanceIdPool ??= _objectPoolProvider.Create(new ComponentsByInstanceIdPoolPolicy(InitialCapacity));

    private class ComponentsByInstanceIdPoolPolicy(int capacity) : PooledObjectPolicy<ConcurrentDictionary<Ulid, AssetId>> {
        public override ConcurrentDictionary<Ulid, AssetId> Create() => new(4, capacity);
        public override bool Return(ConcurrentDictionary<Ulid, AssetId> obj) {
            obj.Clear();
            return true;
        }
    }
    #endregion
}
