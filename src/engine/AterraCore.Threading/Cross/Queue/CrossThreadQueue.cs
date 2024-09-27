// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Common.Attributes.DI;
using AterraCore.Common.Types.Nexities;
using AterraCore.Contracts.Threading2.Cross.Queue;
using JetBrains.Annotations;
using Microsoft.Extensions.ObjectPool;
using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;

namespace AterraCore.Threading.Cross.Queue;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[Singleton<ICrossThreadQueue>]
[UsedImplicitly]
public class CrossThreadQueue : ICrossThreadQueue {
    private readonly ConcurrentQueue<CrossThreadQueueHolder> _renderThreadQueue = new();
    private readonly ConcurrentQueue<CrossThreadQueueHolder> _logicThreadQueue = new();

    #region CrossThreadQueueHolderPool
    private readonly DefaultObjectPoolProvider _objectPoolProvider = new();
    private ObjectPool<CrossThreadQueueHolder>? _queueHolderPool;
    public ObjectPool<CrossThreadQueueHolder> CrossThreadQueueHolderPool => _queueHolderPool ??= _objectPoolProvider.Create(new ComponentsByIdPoolPolicy());

    private class ComponentsByIdPoolPolicy : PooledObjectPolicy<CrossThreadQueueHolder> {
        public override CrossThreadQueueHolder Create() => new();
        public override bool Return(CrossThreadQueueHolder obj) {
            obj.Clear();
            return true;
        }
    }
    #endregion
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    private void Enqueue(ConcurrentQueue<CrossThreadQueueHolder> queue, AssetTag assetTag, IQueueHolder? queueHolder) {
        CrossThreadQueueHolder crossThreadQueueHolder = CrossThreadQueueHolderPool.Get();
        crossThreadQueueHolder.AssetTag = assetTag;
        crossThreadQueueHolder.QueueHolder = queueHolder;
        queue.Enqueue(crossThreadQueueHolder);
    }

    private bool TryDequeue(ConcurrentQueue<CrossThreadQueueHolder> queue, out AssetTag assetTag, out IQueueHolder? queueHolder) {
        assetTag = default;
        queueHolder = null;
        if (!queue.TryDequeue(out CrossThreadQueueHolder? crossThreadQueueHolder)) return false;
        assetTag = crossThreadQueueHolder.AssetTag;
        queueHolder = crossThreadQueueHolder.QueueHolder;
        
        CrossThreadQueueHolderPool.Return(crossThreadQueueHolder);
        return true;
    }
    
    public void EnqueueToRenderThread(AssetTag assetTag, IQueueHolder? queueHolder = null) => Enqueue(_renderThreadQueue, assetTag, queueHolder);
    public bool TryDequeueFromRenderThread(out AssetTag assetTag, [NotNullWhen(true)] out IQueueHolder? queueHolder) => TryDequeue(_renderThreadQueue, out assetTag, out queueHolder);
    
    public void EnqueueToLogicThread(AssetTag assetTag, IQueueHolder? queueHolder = null) => Enqueue(_logicThreadQueue, assetTag, queueHolder);
    public bool TryDequeueFromLogicThread(out AssetTag assetTag, [NotNullWhen(true)] out IQueueHolder? queueHolder) => TryDequeue(_logicThreadQueue, out assetTag, out queueHolder);
}
