// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Contracts.Nexities.Components;
using AterraCore.Contracts.OmniVault.Assets;
using System.Diagnostics.CodeAnalysis;

namespace AterraLib.Nexities.Components;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[Component<IAssetTree>("AterraLib:Nexities/Components/AssetTree")]
[UsedImplicitly]
public class AssetTree : NexitiesComponent, IAssetTree {
    private readonly Dictionary<Ulid, IAssetInstance> _nodesById = new();
    private readonly List<IAssetInstance> _nodes = [];
    private readonly Dictionary<Ulid, List<Ulid>> _children = new();
    private readonly Dictionary<Ulid, Ulid?> _parents = new();
    
    private int? _countCache;
    public int Count => _countCache ??= GetCount();

    // -----------------------------------------------------------------------------------------------------------------
    // Helper Methods
    // -----------------------------------------------------------------------------------------------------------------
    #region GetCount
    private int GetCount() {
        lock (_nodesById) {
            return _nodesById.Count;
        }
    }
    #endregion
    
    #region Snapshot
    private IAssetInstance[] GetSnapshot() {
        lock (_nodes) {
            return _nodes.ToArray();
        }
    }
    #endregion
    
    // -----------------------------------------------------------------------------------------------------------------
    // Of Type Retrieval Methods
    // -----------------------------------------------------------------------------------------------------------------
    #region OfType Methods
    public IEnumerable<T> OfType<T>() where T : IAssetInstance {
        foreach (IAssetInstance node in GetSnapshot()) {
            if (node is T typedItem)
                yield return typedItem;
        }
    }
   
    public IEnumerable<T> OfTypeReverse<T>() where T : IAssetInstance {
        IAssetInstance[] snapshot = GetSnapshot();
        for (int i = snapshot.Length - 1; i >= 0; i--) {
            if (snapshot[i] is T typedItem)
                yield return typedItem;
        }
    }
    public IEnumerable<T> OfTypeMany<T>() where T : IAssetInstance {
        foreach (IAssetInstance node in GetSnapshot()) {
            if (node is T typedItem) yield return typedItem;
            if (node is not IHasAssetTree entities) continue;
            foreach (T assetInstance in entities.AssetTree.OfTypeMany<T>()) {
                yield return assetInstance;
            }
        }
    }
    public IEnumerable<T> OfTypeManyReverse<T>() where T : IAssetInstance {
        IAssetInstance[] snapshot = GetSnapshot();
        for (int i = snapshot.Length - 1; i >= 0; i--) {
            if (snapshot[i] is IHasAssetTree entities)
                foreach (T assetInstance in entities.AssetTree.OfTypeMany<T>())
                    yield return assetInstance;
            if (snapshot[i] is T typedItem)
                yield return typedItem;
        }
    }
    #endregion

    // -----------------------------------------------------------------------------------------------------------------
    // Addition Methods
    // -----------------------------------------------------------------------------------------------------------------
    #region Add
    public void Add<T>(T node, Ulid? parentUlid = null) where T : IAssetInstance {
        lock (_nodesById) {
            lock (_nodes) {
                _nodesById[node.Ulid] = node;
                _nodes.Add(node);
            }

            if (parentUlid == null) return;
            
            _parents[node.Ulid] = parentUlid;
            if (!_children.TryGetValue(parentUlid.Value, out List<Ulid>? value)) {
                value = [];
                _children[parentUlid.Value] = value;
            }
            
            value.Add(node.Ulid);
        }
    }
    #endregion

    #region TryFind
    public bool TryFind(Ulid assetUlid, [NotNullWhen(true)] out IAssetInstance? output) {
        lock (_nodesById) {
            return _nodesById.TryGetValue(assetUlid, out output);
        }
    }
    #endregion

    #region Parent-Child Relationship Methods
    public IEnumerable<IAssetInstance> GetChildren(Ulid parentUlid) {
        lock (_nodesById) {
            if (!_children.TryGetValue(parentUlid, out List<Ulid>? childUlids)) yield break;
            foreach (Ulid ulid in childUlids) {
                if (!_nodesById.TryGetValue(ulid, out IAssetInstance? child)) continue;
                yield return child;
            }
        }
    }

    // Fetch parent of a specific node
    public IAssetInstance? GetParent(Ulid assetUlid) {
        lock (_nodesById) {
            if (!_parents.TryGetValue(assetUlid, out Ulid? parentUlid) || parentUlid == null) return null;
            _nodesById.TryGetValue(parentUlid.Value, out IAssetInstance? parent);
            return parent;
        }

    }
    #endregion
}
