// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Common.Attributes.DI;
using AterraCore.Contracts.Nexities.Systems;
using AterraCore.Contracts.PoolCorps;
using JetBrains.Annotations;
using Microsoft.Extensions.ObjectPool;

namespace AterraCore.PoolCorps;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[UsedImplicitly]
[Singleton<NexitiesSystemPools, INexitiesSystemPools>]
public class NexitiesSystemPools : INexitiesSystemPools {
    private const int InitialCapacity = 12;
    private readonly DefaultObjectPoolProvider _objectPoolProvider = new();

    #region ListPool
    private ObjectPool<List<INexitiesSystem>>? _listPool;
    public ObjectPool<List<INexitiesSystem>> ListPool =>
        _listPool ??= _objectPoolProvider.Create(new ListPoolPolicy(InitialCapacity));

    private class ListPoolPolicy(int capacity) : PooledObjectPolicy<List<INexitiesSystem>> {
        public override List<INexitiesSystem> Create() => new(capacity);
        public override bool Return(List<INexitiesSystem> obj) {
            obj.Clear();
            return true;
        }
    }
    #endregion
    #region LinkedListPool
    private ObjectPool<LinkedList<INexitiesSystem>>? _linkedListPool;
    public ObjectPool<LinkedList<INexitiesSystem>> LinkedListPool =>
        _linkedListPool ??= _objectPoolProvider.Create(new LinkedListPoolPolicy());

    private class LinkedListPoolPolicy : PooledObjectPolicy<LinkedList<INexitiesSystem>> {
        public override LinkedList<INexitiesSystem> Create() => [];
        public override bool Return(LinkedList<INexitiesSystem> obj) {
            obj.Clear();
            return true;
        }
    }
    #endregion
}
