// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace AterraLib.Nexities.Components;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[Component<IAssetTree>("AterraLib:Nexities/Components/AssetTree")]
[UsedImplicitly]
public class AssetTree : NexitiesComponent, IAssetTree {
    public LinkedList<IAssetInstance> Nodes { get; } = [];
    
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public IEnumerable<T> OfType<T>() where T : IAssetInstance => Nodes.OfType<T>();
    public IEnumerable<T> OfTypeReverse<T>() where T : IAssetInstance => Nodes.OfType<T>().Reverse();
    public IEnumerable<T> OfTypeMany<T>() where T : IAssetInstance {
        foreach (T child in Nodes.OfType<T>()) {
            yield return child;
            
            // Has to implement IHasAssetTree
            //      Without this interface, the OfTypeMany wouldn't make sense as we need to pull a flat list from all entities
            if (child is not IHasAssetTree entities) continue;

            foreach (T subChild in entities.ChildEntities.OfTypeMany<T>()) {
                yield return subChild;
            }
        }
    }

    public IEnumerable<T> OfTypeManyReverse<T>() where T : IAssetInstance {
        // Retrieves the last child in the nested layout first
        foreach (T child in Nodes.OfType<T>().Reverse()) {
            if (child is not IHasAssetTree entities) continue;

            foreach (T subChild in entities.ChildEntities.OfTypeManyReverse<T>()) {
                yield return subChild;
            }

            yield return child;
        }
    }
}
