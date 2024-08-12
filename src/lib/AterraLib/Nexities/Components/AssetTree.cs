﻿// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Contracts.Nexities.Components;
using AterraCore.Contracts.OmniVault.Assets;

namespace AterraLib.Nexities.Components;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[Component<IAssetTree>("AterraLib:Nexities/Components/AssetTree")]
[UsedImplicitly]
public class AssetTree : NexitiesComponent, IAssetTree {
    private readonly List<IAssetInstance> _nodes = [];
    
    private int? _countCache;
    public int Count => _countCache ??= GetCount();
    
    private IAssetInstance[]? _cachedSnapshot;
    private IAssetInstance[] UpdateSnapshotCache() {
        lock (_nodes) return _nodes.ToArray();
    }
    private IAssetInstance[] CachedSnapshot => _cachedSnapshot ??= UpdateSnapshotCache();
    
    private IAssetInstance[]? _cachedSnapshotReverse;
    private IAssetInstance[] UpdateSnapshotReverseCache() {
        lock (_nodes) return _nodes.ToArray().Reverse().ToArray();
    }
    private IAssetInstance[] CachedSnapshotReverse => _cachedSnapshotReverse ??= UpdateSnapshotReverseCache();

    // -----------------------------------------------------------------------------------------------------------------
    // Helper Methods
    // -----------------------------------------------------------------------------------------------------------------
    #region GetCount
    private int GetCount() {
        lock (_nodes) {
            return _nodes.Count;
        }
    }
    #endregion
    
    // -----------------------------------------------------------------------------------------------------------------
    // Of Type Retrieval Methods
    // -----------------------------------------------------------------------------------------------------------------
    #region OfType Methods
    public IEnumerable<T> OfType<T>() where T : IAssetInstance {
        lock (CachedSnapshot) {
            foreach (IAssetInstance node in CachedSnapshot)
                if (node is T typedItem) yield return typedItem;
        }
    }
   
    public IEnumerable<T> OfTypeReverse<T>() where T : IAssetInstance {
        lock (CachedSnapshotReverse) {
            foreach (IAssetInstance node in CachedSnapshotReverse)
                if (node is T typedItem) yield return typedItem;
        }
    }
    public IEnumerable<T> OfTypeMany<T>() where T : IAssetInstance {
        lock (CachedSnapshot) {
            foreach (IAssetInstance node in CachedSnapshot) {
                if (node is T typedItem) yield return typedItem;
                if (node is not IHasAssetTree entities) continue;
                foreach (T entity in entities.AssetTree.OfTypeMany<T>())
                    yield return entity;
            }
        }
    }
    public IEnumerable<T> OfTypeManyReverse<T>() where T : IAssetInstance {
        lock (CachedSnapshotReverse) {
            foreach (IAssetInstance node in CachedSnapshotReverse) {
                if (node is IHasAssetTree entities)
                    foreach (T entity in entities.AssetTree.OfTypeManyReverse<T>())
                        yield return entity;
                if (node is T typedItem) yield return typedItem;
            }
        }
    }
    #endregion

    // -----------------------------------------------------------------------------------------------------------------
    // Addition Methods
    // -----------------------------------------------------------------------------------------------------------------
    #region Add
    public void Add<T>(T node) where T : IAssetInstance {
        lock (_nodes) {
            _nodes.Add(node);
        }
        lock (CachedSnapshot) {
            _cachedSnapshot = null;
        }
        lock (CachedSnapshotReverse) {
            _cachedSnapshotReverse = null;
        }
    }
    public void AddFirst<T>(T node) where T : IAssetInstance {
        lock (_nodes) {
            _nodes.Insert(0, node);
        }
        lock (CachedSnapshot) {
            _cachedSnapshot = null;
        }
        lock (CachedSnapshotReverse) {
            _cachedSnapshotReverse = null;
        } 
    }
    #endregion
}
