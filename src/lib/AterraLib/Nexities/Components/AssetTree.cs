// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Contracts.Nexities.Components;
using AterraCore.Contracts.OmniVault.Assets;
using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;

namespace AterraLib.Nexities.Components;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[Component<IAssetTree>("AterraLib:Nexities/Components/AssetTree")]
[UsedImplicitly]
public class AssetTree : NexitiesComponent, IAssetTree {
    private LinkedList<IAssetInstance> Nodes { get; } = [];
    private readonly ReaderWriterLockSlim _cacheLock = new();
    
    private readonly ConcurrentDictionary<(Type, CacheType), LinkedList<IAssetInstance>> _nodeByTypeCache = new();
    
    private const int DebounceIntervalMs = 10; // in ms
    private Timer? _debounceTimer;
    private readonly object _debounceLock = new();

    private int? _countCache;
    public int Count => _countCache ??= GetCount();
    public IEnumerable<IAssetInstance> All => Nodes;
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
        _cacheLock.EnterReadLock();
        try {
            return Nodes.Count;
        } finally {
            _cacheLock.ExitReadLock();
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
            foreach (T subChild in entities.AssetTree.OfTypeManyReverseUnCached<T>()) {
                yield return subChild;
            }
        }
    }
    
    private IEnumerable<T> OfTypeManyReverseInternal<T>() where T : IAssetInstance {
        for (LinkedListNode<IAssetInstance>? currentNode = Nodes.Last; currentNode != null; currentNode = currentNode.Previous) {
            if (currentNode.Value is IHasAssetTree entities) {
                foreach (T subChild in entities.AssetTree.OfTypeManyReverseUnCached<T>()) {
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
        
        if (_nodeByTypeCache.TryGetValue(key, out LinkedList<IAssetInstance>? result)) {
            foreach (T node in result) {
                yield return node;
            }
            yield break;
        }
        
        // Use ConcurrentDictionary GetOrAdd for thread-safe retrieval and addition
        result = _nodeByTypeCache.GetOrAdd(key, _ => {
            LinkedList<IAssetInstance> nodeList = [];
            foreach (T node in callback()) {
                if (node is IAssetInstance castedNode) nodeList.AddLast(castedNode);
            }
            return nodeList;
        });

        foreach (IAssetInstance item in result) {
            yield return (T)item;
        }
    }

    private void ClearCaches() {
        _nodeByTypeCache.Clear();
        _countCache = null;
    }

    private void InvalidateCaches() {
        lock (_debounceLock) {
            if (_debounceTimer != null) {
                _debounceTimer.Change(DebounceIntervalMs, Timeout.Infinite);
                return;
            }
            
            _debounceTimer = new Timer(_ => {
                lock (_debounceLock) {
                    ClearCaches();
                    _debounceTimer = null;
                }
            }, null, DebounceIntervalMs, Timeout.Infinite);
        }
    }
    #endregion
    
    // -----------------------------------------------------------------------------------------------------------------
    // Of Type Retrieval Methods
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

    // -----------------------------------------------------------------------------------------------------------------
    // Addition Methods
    // -----------------------------------------------------------------------------------------------------------------
    #region AddLast
    public void AddLast<T>(T node) where T : IAssetInstance {
        _cacheLock.EnterWriteLock();
        try {
            Nodes.AddLast(node);
        } finally {
            _cacheLock.ExitWriteLock();
        }
        InvalidateCaches();
    }
    #endregion
    #region AddFirst
    public void AddFirst<T>(T node) where T : IAssetInstance {
        _cacheLock.EnterWriteLock();
        try {
            Nodes.AddFirst(node);
        } finally {
            _cacheLock.ExitWriteLock();
        }
        InvalidateCaches();
    }
    #endregion
    #region First & TryGetFirst
    public IAssetInstance? First => TryGetFirst(out IAssetInstance? output) ? output : null;
    public bool TryGetFirst<T>([NotNullWhen(true)] out T? output) where T : class, IAssetInstance {
        _cacheLock.EnterReadLock();
        output = default;
        try {
            IAssetInstance? node = Nodes.First?.Value;
            output = node as T;
            return output is not null;
        } finally {
            _cacheLock.ExitReadLock();
        }
    }
    #endregion
    #region Last & TryGetLast
    public IAssetInstance? Last => TryGetLast(out IAssetInstance? output) ? output : null;
    public bool TryGetLast<T>([NotNullWhen(true)] out T? output) where T : class, IAssetInstance {
        _cacheLock.EnterReadLock();
        output = default;
        try {
            IAssetInstance? node = Nodes.Last?.Value;
            output = node as T;
            return output is not null;
        } finally {
            _cacheLock.ExitReadLock();
        }
    }
    #endregion
    #region TryAddAfter
    public bool TryAddAfter<T>(Guid assetGuid, T newAsset) where T : IAssetInstance {
        _cacheLock.EnterUpgradeableReadLock();
        try {
            if (Nodes.Find(a => a.Guid == assetGuid) is not {} node) return false;
            _cacheLock.EnterWriteLock();
            try {
                Nodes.AddAfter(node, newAsset);
                return true;
            } finally {
                _cacheLock.ExitWriteLock();
            }
        } finally {
            _cacheLock.ExitUpgradeableReadLock();
        }
    }
    #endregion
    #region TryAddBefore
    public bool TryAddBefore<T>(Guid assetGuid, T newAsset) where T : IAssetInstance {
        _cacheLock.EnterUpgradeableReadLock();
        try {
            if (Nodes.Find(a => a.Guid == assetGuid) is not {} node) return false;
            _cacheLock.EnterWriteLock();
            try {
                Nodes.AddBefore(node, newAsset);
                return true;
            } finally {
                _cacheLock.ExitWriteLock();
            }
        } finally {
            _cacheLock.ExitUpgradeableReadLock();
        }
    }
    #endregion
    #region TryFind
    public bool TryFind(Guid assetGuid, [NotNullWhen(true)] out LinkedListNode<IAssetInstance>? output) {
        output = Nodes.Find(a => a.Guid == assetGuid);
        return output is not null;
    }
    public bool TryFind(Guid assetGuid, [NotNullWhen(true)] out IAssetInstance? output) {
        output = Nodes.Find(a => a.Guid == assetGuid)?.Value;
        return output is not null;
    }
    public bool TryFind<T>(Guid assetGuid, [NotNullWhen(true)] out T? output) where T : class, IAssetInstance {
        IAssetInstance? node = Nodes.Find(a => a.Guid == assetGuid)?.Value;
        output = node as T;
        return output is not null;
    }
    #endregion
}
