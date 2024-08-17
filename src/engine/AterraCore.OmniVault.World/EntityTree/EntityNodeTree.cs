// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Contracts.OmniVault.Assets;
using AterraCore.Contracts.OmniVault.World.EntityTree;
using AterraCore.OmniVault.World.EntityTree.Pools;

namespace AterraCore.OmniVault.World.EntityTree;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
/// <summary>
/// The `EntityNodeTree` struct represents a hierarchical tree structure of entity nodes in an entity system.
/// </summary>
public readonly struct EntityNodeTree(IEntityNode root, IEntityTreePools poolProvider) : IEntityNodeTree {
    /// <summary>
    /// Represents a tree structure of entity nodes with a root node.
    /// </summary>
    private IEntityNode Root { get; } = root;
    
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    #region Flat | root -> child -> n-grandchildren
    /// <summary>
    /// Retrieves all assets in the entity tree as a flattened collection.
    /// </summary>
    /// <returns>A collection of asset instances in the entity tree.</returns>
    public IEnumerable<IAssetInstance> GetAsFlat() {
        using var stackPool = new PooledResource<Stack<IEntityNode>>(poolProvider.StackPool);
        
        Stack<IEntityNode> stack = stackPool.Item;
        
        stack.Push(Root);
        
        while (stack.TryPop(out IEntityNode? currentNode)){
            yield return currentNode.Value;

            for (int i = currentNode.Children.Count - 1; i >= 0; i--) 
                stack.Push(currentNode.Children[i]);
        }
        
    }
    #endregion
    #region FlatReverse | deepest n-grandchild -> child -> root
    /// <summary>
    /// Retrieves all assets in the entity tree in reverse order as a flattened collection.
    /// </summary>
    /// <returns>A collection of asset instances in the entity tree in reverse order.</returns>
    public IEnumerable<IAssetInstance> GetAsFlatReverse() {
        using var queuePool = new PooledResource<Queue<IEntityNode>>(poolProvider.QueuePool);
        using var assetsPool = new PooledResource<Queue<IAssetInstance>>(poolProvider.QueueAssetInstancePool);
        
        Queue<IEntityNode> queue = queuePool.Item;
        Queue<IAssetInstance> assets = assetsPool.Item;

        queue.Enqueue(Root);
        
        while (queue.TryDequeue(out IEntityNode? currentNode)) {
            assets.Enqueue(currentNode.Value);

            foreach (IEntityNode child in currentNode.Children) 
                queue.Enqueue(child);
        }

        while(assets.TryDequeue(out IAssetInstance? asset)) 
            yield return asset;
    }
    #endregion
    #region FlatWithParent | root -> child -> n-grandchildren
    /// <summary>
    /// Retrieves all assets in the entity tree as a flattened collection along with their parent assets.
    /// </summary>
    /// <returns>A collection of tuples, each containing a parent asset instance and its child asset instance.</returns>
    public IEnumerable<(IAssetInstance? Parent, IAssetInstance Child)> GetAsFlatWithParent() {
        using var stackPool = new PooledResource<Stack<(IEntityNode? Parent, IEntityNode Child)>>(poolProvider.ParentedStackPool);
        
        Stack<(IEntityNode? Parent, IEntityNode Child)> stack = stackPool.Item;
        
        stack.Push((null, Root));
        
        while (stack.TryPop(out (IEntityNode? Parent, IEntityNode Node) t)) {
            (IEntityNode? parent, IEntityNode child) = t;
            yield return (parent?.Value, child.Value);

            for (int i = child.Children.Count -1 ; i >= 0; i--)
                stack.Push((child, child.Children[i]));
        }
    }
    #endregion
    #region FlatReverseWithParent | deepest n-grandchild -> child -> root
    /// <summary>
    /// Retrieves all assets in the entity tree as a flattened reverse representation with their parent-child relationship.
    /// </summary>
    /// <returns>
    /// An <see cref="IEnumerable{T}"/> of tuples representing the parent-child relationship in reverse order.
    /// Each tuple contains an <see cref="IAssetInstance"/> representing the possible existing parent node and an <see cref="IAssetInstance"/> representing the child node.
    /// </returns>
    public IEnumerable<(IAssetInstance? Parent, IAssetInstance Child)> GetAsFlatReverseWithParent() {
        using var stackPool = new PooledResource<Stack<(IEntityNode? Parent, IEntityNode Node)>>(poolProvider.ParentedStackPool);
        using var assetsPool = new PooledResource<Stack<(IAssetInstance? Parent, IAssetInstance Child)>>(poolProvider.ParentedStackAssetInstancePool);
        
        // I'm using the usefulness of stacks, LIFO to just get all of them after the fact
        Stack<(IEntityNode? Parent, IEntityNode Node)> stack = stackPool.Item;
        Stack<(IAssetInstance? Parent, IAssetInstance Child)> assets = assetsPool.Item;

        stack.Push((null, Root));
        
        while (stack.TryPop(out (IEntityNode? Parent, IEntityNode Node) t)) {
            (IEntityNode? parent, IEntityNode child) = t;
            assets.Push((parent?.Value, child.Value));
            
            foreach (IEntityNode grandChild in child.Children) {
                stack.Push((child, grandChild));
            }
        }

        while (assets.TryPop(out (IAssetInstance? Parent, IAssetInstance Child) asset))
            yield return asset;
    }
    #endregion
    
    
    
    
}
