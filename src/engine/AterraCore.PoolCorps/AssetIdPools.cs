// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Common.Attributes.DI;
using AterraCore.Common.Types.Nexities;
using AterraCore.Contracts.PoolCorps;
using JetBrains.Annotations;
using Microsoft.Extensions.ObjectPool;
using System.Collections.Concurrent;

namespace AterraCore.PoolCorps;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[UsedImplicitly]
[Singleton<IAssetIdPools>]
public class AssetIdPools : IAssetIdPools {
    private const int InitialCapacity = 64;
    private readonly DefaultObjectPoolProvider _objectPoolProvider = new();

    #region AssetIdListPool
    private ObjectPool<List<AssetId>>? _ulidListPool;
    public ObjectPool<List<AssetId>> AssetIdListPool =>
        _ulidListPool ??= _objectPoolProvider.Create(new AssetIdListPoolPolicy(InitialCapacity));

    private class AssetIdListPoolPolicy(int capacity) : PooledObjectPolicy<List<AssetId>> {
        public override List<AssetId> Create() => new(capacity);
        public override bool Return(List<AssetId> obj) {
            obj.Clear();
            return true;
        }
    }
    #endregion
    #region AssetIdHashSetPool
    private ObjectPool<HashSet<AssetId>>? _ulidHashSetPool;
    public ObjectPool<HashSet<AssetId>> AssetIdHashSetPool =>
        _ulidHashSetPool ??= _objectPoolProvider.Create(new AssetIdHashSetPoolPolicy(InitialCapacity));

    private class AssetIdHashSetPoolPolicy(int capacity) : PooledObjectPolicy<HashSet<AssetId>> {
        public override HashSet<AssetId> Create() => new(capacity);
        public override bool Return(HashSet<AssetId> obj) {
            obj.Clear();
            return true;
        }
    }
    #endregion
    #region AssetIdConcurrentStackPool
    private ObjectPool<ConcurrentStack<AssetId>>? _ulidConcurrentStackPool;
    public ObjectPool<ConcurrentStack<AssetId>> AssetIdConcurrentStackPool =>
        _ulidConcurrentStackPool ??= _objectPoolProvider.Create(new AssetIdConcurrentStackPoolPolicy());

    private class AssetIdConcurrentStackPoolPolicy : PooledObjectPolicy<ConcurrentStack<AssetId>> {
        public override ConcurrentStack<AssetId> Create() => new();
        public override bool Return(ConcurrentStack<AssetId> obj) {
            obj.Clear();
            return true;
        }
    }
    #endregion
}
