// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Collections.Concurrent;

namespace AterraLib.Nexities.Components;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[Component<IAssetTree>("AterraLib:Nexities/Components/AssetTree")]
[UsedImplicitly]
public class AssetTree : NexitiesComponent, IAssetTree {
    public LinkedList<IAssetInstance> Nodes { get; } = [];
    private readonly ReaderWriterLockSlim _lock = new();
    public int Count {
        get {
            _lock.EnterReadLock();
            try {
                return Nodes.Count;
            }
            finally {
                _lock.ExitReadLock();
            }
        }
    }
                                            //   many  reverse
    private readonly ConcurrentDictionary<(Type, bool, bool), LinkedList<IAssetInstance>> _nodeByTypeCache = new();
    
    // -----------------------------------------------------------------------------------------------------------------
    // Helper Methods
    // -----------------------------------------------------------------------------------------------------------------
    #region OfType & OfTypeReverse Helper Methods
    private static IEnumerable<T> OfTypeInternal<T>(LinkedList<IAssetInstance> nodes) where T : IAssetInstance {
        LinkedListNode<IAssetInstance>? currentNode = nodes.First;
        while (currentNode != null) {
            if (currentNode.Value is T typedNode)
                yield return typedNode;
            currentNode = currentNode.Next;
        }
    }
    
    private static IEnumerable<T> OfTypeReverseInternal<T>(LinkedList<IAssetInstance> nodes) where T : IAssetInstance {
        LinkedListNode<IAssetInstance>? currentNode = nodes.Last;
        while (currentNode != null) {
            if (currentNode.Value is T typedNode)
                yield return typedNode;
            currentNode = currentNode.Previous;
        }
    }
    
    private static IEnumerable<T> OfTypeManyInternal<T>(LinkedList<IAssetInstance> nodes) where T : IAssetInstance {
        LinkedListNode<IAssetInstance>? currentNode = nodes.First;
        while (currentNode != null) {
            if (currentNode.Value is T nodeType) yield return nodeType;
            if (currentNode.Value is not IHasAssetTree entities) continue;
            foreach (T subChild in OfTypeManyInternal<T>(entities.ChildEntities.Nodes)) {
                yield return subChild;
            }
            currentNode = currentNode.Next;
        }
        foreach (IAssetInstance node in nodes) {
            
            
        }
    }
    private static IEnumerable<T> OfTypeManyReverseInternal<T>(LinkedList<IAssetInstance> nodes) where T : IAssetInstance {
        LinkedListNode<IAssetInstance>? currentNode = nodes.Last;
        while (currentNode != null) {
            if (currentNode.Value is IHasAssetTree entities) {
                foreach (T subChild in OfTypeManyReverseInternal<T>(entities.ChildEntities.Nodes)) {
                    yield return subChild;
                }
            }
            if (currentNode.Value is T nodeType) yield return nodeType;

            currentNode = currentNode.Previous;
        }
    }
    #endregion

    private IEnumerable<T> GetOrAddToCache<T>(bool many, bool reverse, Func<LinkedList<IAssetInstance>, IEnumerable<T>> callback) {
        (Type, bool, bool) key = (typeof(T), many, reverse);
        
        if (!_nodeByTypeCache.TryGetValue(key, out LinkedList<IAssetInstance>? result)) {
            IEnumerable<T> output = callback(Nodes);
            result = [];
            foreach (T node in output) {
                yield return node;
                result.AddLast((IAssetInstance)node!);
            }
            
            _nodeByTypeCache.TryAdd(key, result);
            yield break;
        }
        foreach (T node in result) {
            yield return node;
        }
    }

    private void InvalidateCaches<T>() {
        _nodeByTypeCache.TryRemove((typeof(T), false, false), out _);
        _nodeByTypeCache.TryRemove((typeof(T), false, true), out _);
        _nodeByTypeCache.TryRemove((typeof(T), true, false), out _);
        _nodeByTypeCache.TryRemove((typeof(T), true, true), out _);
    }

    // -----------------------------------------------------------------------------------------------------------------
    // Retrieval Methods
    // -----------------------------------------------------------------------------------------------------------------
    public IEnumerable<T> OfType<T>() where T : IAssetInstance =>            GetOrAddToCache(false, false, OfTypeInternal<T>);
    public IEnumerable<T> OfTypeReverse<T>() where T : IAssetInstance =>     GetOrAddToCache(false, true, OfTypeReverseInternal<T>);
    public IEnumerable<T> OfTypeMany<T>() where T : IAssetInstance =>        GetOrAddToCache(true, false, OfTypeManyInternal<T>);
    public IEnumerable<T> OfTypeManyReverse<T>() where T : IAssetInstance => GetOrAddToCache(true, true, OfTypeManyReverseInternal<T>);

    // -----------------------------------------------------------------------------------------------------------------
    // Addition Methods
    // -----------------------------------------------------------------------------------------------------------------
    public void AddLast<T>(T node) where T : IAssetInstance {
        _lock.EnterWriteLock();
        try {
            Nodes.AddLast(node);
            InvalidateCaches<T>();
        }
        finally {
            _lock.ExitWriteLock();
        }
    }
    
    public void AddFirst<T>(T node) where T : IAssetInstance {
        _lock.EnterWriteLock();
        try {
            Nodes.AddFirst(node);
            InvalidateCaches<T>();
        }
        finally {
            _lock.ExitWriteLock();
        }
    }
}
