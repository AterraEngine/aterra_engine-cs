// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Contracts.Nexities.Components;
using AterraCore.Contracts.OmniVault.Assets;
using Pools;
using System.Buffers;
using System.Collections;
using System.Diagnostics.CodeAnalysis;

namespace AterraLib.Nexities.Components;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[Component<IAssetTree>("AterraLib:Nexities/Components/AssetTree")]
[UsedImplicitly]
public class AssetTree : NexitiesComponent, IAssetTree {
    private LinkedList<IAssetInstance> Nodes { get; } = [];
    private readonly ReaderWriterLockSlim _cacheLock = new(LockRecursionPolicy.SupportsRecursion);
    
    private static readonly Dictionary<(Type, CacheType), Delegate> DelegateCache = new();
    
    private readonly Dictionary<(Type, CacheType), List<object>> _typeCache = new();
    
    private const int DebounceIntervalMs = 10; // in ms
    private Timer? _debounceTimer;
    private readonly object _debounceLock = new();

    private int? _countCache;
    public int Count => _countCache ??= GetCount();
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
    private static List<T> OfTypeInternal<T>(LinkedList<IAssetInstance> nodes) where T : IAssetInstance {
        List<T> result = ListPool<T>.Get();
        try {
            for (LinkedListNode<IAssetInstance>? currentNode = nodes.First; currentNode != null; currentNode = currentNode.Next) {
                if (currentNode.Value is T typedNode) result.Add(typedNode);
            }
        }
        finally {
            result.TrimExcess();
        }
        return result;
    }
    
    private static List<T> OfTypeReverseInternal<T>(LinkedList<IAssetInstance> nodes) where T : IAssetInstance {
        List<T> result = ListPool<T>.Get();
        try {
            for (LinkedListNode<IAssetInstance>? currentNode = nodes.Last; currentNode != null; currentNode = currentNode.Previous) {
                if (currentNode.Value is T typedNode) result.Add(typedNode);
            }
        }
        finally {
            result.TrimExcess();
        }
        return result;
    }
    
    private static List<T> OfTypeManyInternal<T>(LinkedList<IAssetInstance> nodes) where T : IAssetInstance {
        List<T> result = ListPool<T>.Get();
        try {
            for (LinkedListNode<IAssetInstance>? currentNode = nodes.First; currentNode != null; currentNode = currentNode.Next) {
                if (currentNode.Value is T nodeType) result.Add(nodeType);
                if (currentNode.Value is IHasAssetTree entities) result.AddRange(entities.AssetTree.OfTypeMany<T>());
            }
        }
        finally {
            result.TrimExcess();
        }
        return result;
    }
    
    private static List<T> OfTypeManyReverseInternal<T>(LinkedList<IAssetInstance> nodes) where T : IAssetInstance {
        List<T> result = ListPool<T>.Get();
        try {
            for (LinkedListNode<IAssetInstance>? currentNode = nodes.Last; currentNode != null; currentNode = currentNode.Previous) {
                if (currentNode.Value is IHasAssetTree entities) result.AddRange(entities.AssetTree.OfTypeManyReverse<T>());
                if (currentNode.Value is T nodeType) result.Add(nodeType);
            }
        }
        finally {
            result.TrimExcess();
        }
        return result;
    }
    #endregion
    #region Caching helpers
    private void ClearCaches() {
        _cacheLock.EnterWriteLock();
        try {
            _typeCache.Clear();
            _countCache = null;
        } finally {
            _cacheLock.ExitWriteLock();
        }
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
    
    private List<T> GetOrAddToCacheWithCachedDelegate<T>(CacheType cacheType, Func<LinkedList<IAssetInstance>, IEnumerable<T>> callback, LinkedList<IAssetInstance> nodes) where T : IAssetInstance {
        (Type, CacheType) key = (typeof(T), cacheType);

        _cacheLock.EnterUpgradeableReadLock();
        try {
            if (DelegateCache.TryGetValue(key, out Delegate? cachedDelegate) 
                && cachedDelegate is Func<LinkedList<IAssetInstance>, IEnumerable<T>> convertedCallback) {
                return GetOrYieldFromCache(cacheType, convertedCallback, nodes);
            }

            _cacheLock.EnterWriteLock();
            try {
                Func<LinkedList<IAssetInstance>, IEnumerable<T>> internalMethod = callback;
                DelegateCache[key] = internalMethod;
                cachedDelegate = internalMethod;
            } finally {
                _cacheLock.ExitWriteLock();
            }

            return GetOrYieldFromCache(cacheType, (Func<LinkedList<IAssetInstance>, IEnumerable<T>>)cachedDelegate, nodes);
        } finally {
            _cacheLock.ExitUpgradeableReadLock();
        }
    }

    private List<T> GetOrYieldFromCache<T>(CacheType cacheType, Func<LinkedList<IAssetInstance>, IEnumerable<T>> callback, LinkedList<IAssetInstance> nodes) where T : IAssetInstance {
        (Type, CacheType) key = (typeof(T), cacheType);
        List<T>? cachedList;
        T[]? pooledArray = null;

        _cacheLock.EnterUpgradeableReadLock();
        try {
            if (!_typeCache.TryGetValue(key, out List<object>? objectList)) {
                _cacheLock.EnterWriteLock();
                try {
                    objectList = callback(nodes).Cast<object>().ToList();
                    _typeCache[key] = objectList;
                } finally {
                    _cacheLock.ExitWriteLock();
                }
            }

            // Pool an array for the temporary operation
            pooledArray = ArrayPool<T>.Shared.Rent(objectList.Count);

            int i = 0;
            foreach (object item in objectList)
                if (item is T typedItem) pooledArray[i++] = typedItem;

            cachedList = [..pooledArray.AsSpan(0, i).ToArray()];
        } finally {
            _cacheLock.ExitUpgradeableReadLock();
            if (pooledArray != null) ArrayPool<T>.Shared.Return(pooledArray);
        }

        return cachedList;
    }
    #endregion
    
    // -----------------------------------------------------------------------------------------------------------------
    // Of Type Retrieval Methods
    // -----------------------------------------------------------------------------------------------------------------
    # region OfType… Cached
    private const CacheType CtOfType =            CacheType.None;
    private const CacheType CtOfTypeReverse =     CacheType.Reverse;
    private const CacheType CtOfTypeMany =        CacheType.Many;
    private const CacheType CtOfTypeManyReverse = CacheType.Many | CacheType.Reverse;
    
    public IEnumerable<T> OfType<T>() where T : IAssetInstance =>                GetOrAddToCacheWithCachedDelegate(CtOfType, OfTypeInternal<T>, Nodes);
    public IEnumerable<T> OfTypeReverse<T>() where T : IAssetInstance =>         GetOrAddToCacheWithCachedDelegate(CtOfTypeReverse, OfTypeReverseInternal<T>, Nodes);
    public IEnumerable<T> OfTypeMany<T>() where T : IAssetInstance =>            GetOrAddToCacheWithCachedDelegate(CtOfTypeMany, OfTypeManyInternal<T>, Nodes);
    public IEnumerable<T> OfTypeManyReverse<T>() where T : IAssetInstance =>     GetOrAddToCacheWithCachedDelegate(CtOfTypeManyReverse, OfTypeManyReverseInternal<T>, Nodes);
    # endregion
    
    # region OfType… Local
    public IEnumerable<T> OfTypeLocal<T>() where T : IAssetInstance =>            OfTypeInternal<T>(Nodes);
    public IEnumerable<T> OfTypeReverseLocal<T>() where T : IAssetInstance =>     OfTypeReverseInternal<T>(Nodes);
    public IEnumerable<T> OfTypeManyLocal<T>() where T : IAssetInstance =>        OfTypeManyInternal<T>(Nodes);
    public IEnumerable<T> OfTypeManyReverseLocal<T>() where T : IAssetInstance => OfTypeManyReverseInternal<T>(Nodes);
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
    public bool TryAddAfter<T>(Ulid assetUlid, T newAsset) where T : IAssetInstance {
        _cacheLock.EnterUpgradeableReadLock();
        try {
            if (Nodes.Find(a => a.Ulid == assetUlid) is not {} node) return false;
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
    public bool TryAddBefore<T>(Ulid assetUlid, T newAsset) where T : IAssetInstance {
        _cacheLock.EnterUpgradeableReadLock();
        try {
            if (Nodes.Find(a => a.Ulid == assetUlid) is not {} node) return false;
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
    public bool TryFind(Ulid assetUlid, [NotNullWhen(true)] out LinkedListNode<IAssetInstance>? output) {
        output = Nodes.Find(a => a.Ulid == assetUlid);
        return output is not null;
    }
    public bool TryFind(Ulid assetUlid, [NotNullWhen(true)] out IAssetInstance? output) {
        output = Nodes.Find(a => a.Ulid == assetUlid)?.Value;
        return output is not null;
    }
    public bool TryFind<T>(Ulid assetUlid, [NotNullWhen(true)] out T? output) where T : class, IAssetInstance {
        IAssetInstance? node = Nodes.Find(a => a.Ulid == assetUlid)?.Value;
        output = node as T;
        return output is not null;
    }
    #endregion
}
