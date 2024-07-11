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
    private LinkedList<IAssetInstance> Nodes { get; } = [];
    private readonly ReaderWriterLockSlim _lock = new();
    
    private int? _countCache;
    public int Count => _countCache ??= GetCount();
    private readonly ConcurrentDictionary<(Type, CacheType), LinkedList<IAssetInstance>> _nodeByTypeCache = new();
    
    private const int DebounceIntervalMs = 10; // in ms
    private Timer? _debounceTimer;
    private readonly object _debounceLock = new();

    // -----------------------------------------------------------------------------------------------------------------
    // Helper Classes (this is only out of necessity)
    // -----------------------------------------------------------------------------------------------------------------
    #region CacheType
    [Flags]
    private enum CacheType {
        None = 0,
        Many = 1 << 01,
        Reverse = 1 << 02
    }
    #endregion
    
    // -----------------------------------------------------------------------------------------------------------------
    // Helper Methods
    // -----------------------------------------------------------------------------------------------------------------
    #region GetCount
    private int GetCount() {
        _lock.EnterReadLock();
        try {
            return Nodes.Count;
        } finally {
            _lock.ExitReadLock();
        }
    }
    #endregion
    #region OfType & OfTypeReverse Helper Methods
    private IEnumerable<T> OfTypeInternal<T>() where T : IAssetInstance {
        for (LinkedListNode<IAssetInstance>? currentNode = Nodes.First; currentNode != null; currentNode = currentNode.Next) {
            if (currentNode.Value is T typedNode) yield return typedNode;
        }
    }
    
    private IEnumerable<T> OfTypeReverseInternal<T>() where T : IAssetInstance {
        for (LinkedListNode<IAssetInstance>? currentNode = Nodes.Last; currentNode != null; currentNode = currentNode.Previous) {
            if (currentNode.Value is T typedNode) yield return typedNode;
        }
    }
    
    private IEnumerable<T> OfTypeManyInternal<T>() where T : IAssetInstance {
        for (LinkedListNode<IAssetInstance>? currentNode = Nodes.First; currentNode != null; currentNode = currentNode.Next) {
            if (currentNode.Value is T nodeType) yield return nodeType;
            if (currentNode.Value is not IHasAssetTree entities) continue;
            foreach (T subChild in entities.ChildEntities.OfTypeManyReverseUnCached<T>()) {
                yield return subChild;
            }
        }
    }
    
    private IEnumerable<T> OfTypeManyReverseInternal<T>() where T : IAssetInstance {
        for (LinkedListNode<IAssetInstance>? currentNode = Nodes.Last; currentNode != null; currentNode = currentNode.Previous) {
            if (currentNode.Value is IHasAssetTree entities) {
                foreach (T subChild in entities.ChildEntities.OfTypeManyReverseUnCached<T>()) {
                    yield return subChild;
                }
            }
            if (currentNode.Value is T nodeType) yield return nodeType;
        }
    }
    #endregion
    #region Caching helpers
    private IEnumerable<T> GetOrAddToCache<T>(CacheType cacheType, Func<IEnumerable<T>> callback) {
        (Type, CacheType) key = (typeof(T), cacheType);
        _lock.EnterUpgradeableReadLock();
        try {
            if (_nodeByTypeCache.TryGetValue(key, out LinkedList<IAssetInstance>? result)) {
                foreach (T node in result) {
                    yield return node;
                }
                yield break;
            }
            
            _lock.EnterWriteLock();
            try {
                result = [];
                foreach (T node in callback()) {
                    if (node is IAssetInstance castedNode) result.AddLast(castedNode);
                }

                _nodeByTypeCache[key] = result;
            } finally {
                _lock.ExitWriteLock();
            }
            
            foreach (IAssetInstance item in result) {
                yield return (T)item;
            }
        }
        finally {
            _lock.ExitUpgradeableReadLock();
        }
    }

    private void InvalidateCaches() {
        lock (_debounceLock) {
            if (_debounceTimer != null) {
                _debounceTimer.Change(DebounceIntervalMs, Timeout.Infinite);
                return;
            }
            
            _debounceTimer = new Timer(
                _ => {
                    lock (_debounceLock) {
                        _nodeByTypeCache.Clear();
                        _countCache = null;
                        _debounceTimer = null;
                    }
                },
                state: null,
                (long)DebounceIntervalMs,
                Timeout.Infinite
            );
        }
    }
    #endregion
    
    // -----------------------------------------------------------------------------------------------------------------
    // Retrieval Methods
    // -----------------------------------------------------------------------------------------------------------------
    # region OfType… Cached
    public IEnumerable<T> OfType<T>() where T : IAssetInstance =>                    GetOrAddToCache(CacheType.None, OfTypeInternal<T>);
    public IEnumerable<T> OfTypeReverse<T>() where T : IAssetInstance =>             GetOrAddToCache(CacheType.Reverse, OfTypeReverseInternal<T>);
    public IEnumerable<T> OfTypeMany<T>() where T : IAssetInstance =>                GetOrAddToCache(CacheType.Many, OfTypeManyInternal<T>);
    public IEnumerable<T> OfTypeManyReverse<T>() where T : IAssetInstance =>         GetOrAddToCache(CacheType.Many | CacheType.Reverse, OfTypeManyReverseInternal<T>);
    # endregion
    # region OfType… UnCached
    public IEnumerable<T> OfTypeUnCached<T>() where T : IAssetInstance =>            OfTypeInternal<T>();
    public IEnumerable<T> OfTypeReverseUnCached<T>() where T : IAssetInstance =>     OfTypeReverseInternal<T>();
    public IEnumerable<T> OfTypeManyUnCached<T>() where T : IAssetInstance =>        OfTypeManyInternal<T>();
    public IEnumerable<T> OfTypeManyReverseUnCached<T>() where T : IAssetInstance => OfTypeManyReverseInternal<T>();
    # endregion

    public IEnumerable<IAssetInstance> All => Nodes;

    // -----------------------------------------------------------------------------------------------------------------
    // Addition Methods
    // -----------------------------------------------------------------------------------------------------------------
    public void AddLast<T>(T node) where T : IAssetInstance {
        _lock.EnterWriteLock();
        try {
            Nodes.AddLast(node);
        } finally {
            _lock.ExitWriteLock();
        }
        InvalidateCaches();
    }
    
    public void AddFirst<T>(T node) where T : IAssetInstance {
        _lock.EnterWriteLock();
        try {
            Nodes.AddFirst(node);
        } finally {
            _lock.ExitWriteLock();
        }
        InvalidateCaches();
    }
}
