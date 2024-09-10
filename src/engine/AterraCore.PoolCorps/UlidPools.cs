// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Common.Attributes;
using AterraCore.Contracts.PoolCorps;
using JetBrains.Annotations;
using Microsoft.Extensions.ObjectPool;

namespace AterraCore.PoolCorps;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[UsedImplicitly]
[Singleton<IUlidPools>]
public class UlidPools : IUlidPools {
    private const int InitialCapacity = 24;
    private readonly DefaultObjectPoolProvider _objectPoolProvider = new();

    #region XmlNodeQueuePool
    private ObjectPool<HashSet<Ulid>>? _ulidHashSetPool;
    public ObjectPool<HashSet<Ulid>> UlidHashSetPool =>
        _ulidHashSetPool ??= _objectPoolProvider.Create(new UlidHashSetPoolPolicy(InitialCapacity));

    private class UlidHashSetPoolPolicy(int capacity) : PooledObjectPolicy<HashSet<Ulid>> {
        public override HashSet<Ulid> Create() => new(capacity);
        public override bool Return(HashSet<Ulid> obj) {
            obj.Clear();
            return true;
        }
    }
    #endregion
}
