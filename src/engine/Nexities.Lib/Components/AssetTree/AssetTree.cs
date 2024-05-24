// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

using AterraCore.Contracts.Nexities.Assets;
using AterraCore.Contracts.Nexities.Components.AssetTree;
using AterraCore.Nexities.Components;
using JetBrains.Annotations;
namespace Nexities.Lib.Components.AssetTree;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[Component<IAssetTree>("AC00-0003")]
[UsedImplicitly]
public class AssetTree : NexitiesComponent, IAssetTree {
    private readonly LinkedList<IAssetInstance> _nodes = [];
    public IEnumerable<IAssetInstance> Nodes => _nodes;

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public IEnumerable<T> OfType<T>() where T : IAssetInstance => _nodes.OfType<T>();
    public IEnumerable<T> OfTypeReverse<T>() where T : IAssetInstance => _nodes.OfType<T>().Reverse();

    public IEnumerable<T> OfTypeMany<T>() where T : IAssetInstance {
        // Has to implement IHasAssetTree
        //      Without this interface, the OfTypeMany wouldn't make sense as we need to pull a flat list from all entities

        foreach (T child in _nodes.OfType<T>()) {
            yield return child;

            if (child is not IHasAssetTree entities) continue;

            foreach (T subChild in entities.ChildEntities.OfTypeMany<T>()) {
                yield return subChild;
            }
        }
    }

    public IEnumerable<T> OfTypeManyReverse<T>() where T : IAssetInstance {
        // Retrieves the last child in the nested layout first
        foreach (T child in _nodes.OfType<T>().Reverse()) {
            if (child is not IHasAssetTree entities) continue;

            foreach (T subChild in entities.ChildEntities.OfTypeManyReverse<T>()) {
                yield return subChild;
            }

            yield return child;
        }
    }

    public bool TryAddNode(IAssetInstance assetInstance) {
        _nodes.AddLast(assetInstance);
        return true; // This doesn't make sense, review this in the future
    }
}