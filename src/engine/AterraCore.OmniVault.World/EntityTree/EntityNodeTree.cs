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
public readonly struct EntityNodeTree(IEntityNode root, IEntityTreePools entityTreePools) : IEntityNodeTree {
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
        using var stackPool = new PooledResource<Stack<IEntityNode>>(entityTreePools.StackPool);
        Stack<IEntityNode> stack = stackPool.Item;
        
        stack.Push(Root);
        while (stack.Count > 0) {
            IEntityNode currentNode = stack.Pop();
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
        using var queuePool = new PooledResource<Queue<IEntityNode>>(entityTreePools.QueuePool);
        using var assetsPool = new PooledResource<List<IAssetInstance>>(entityTreePools.ListPool);
        Queue<IEntityNode> queue = queuePool.Item;
        List<IAssetInstance> assets = assetsPool.Item;

        queue.Enqueue(Root);
        while (queue.Count > 0) {
            IEntityNode currentNode = queue.Dequeue();
            assets.Add(currentNode.Value);

            foreach (IEntityNode child in currentNode.Children) 
                queue.Enqueue(child);
        }

        for (int i = assets.Count - 1; i >= 0; i--)
            yield return assets[i];
    }
    #endregion
    #region FlatWithParent | root -> child -> n-grandchildren
    /// <summary>
    /// Retrieves all assets in the entity tree as a flattened collection along with their parent assets.
    /// </summary>
    /// <returns>A collection of tuples, each containing a parent asset instance and its child asset instance.</returns>
    public IEnumerable<(IAssetInstance? Parent,IAssetInstance Child)> GetAsFlatWithParent() {
        using var stackPool = new PooledResource<Stack<(IEntityNode? Parent, IEntityNode Child)>>(entityTreePools.ParentedStackPool);
        Stack<(IEntityNode? Parent, IEntityNode Child)> stack = stackPool.Item;
        
        stack.Push((null, Root));
        while (stack.Count > 0) {
            (IEntityNode? Parent, IEntityNode Child) currentNode = stack.Pop();
            yield return (currentNode.Parent?.Value, currentNode.Child.Value);

            for (int i = currentNode.Child.Children.Count -1 ; i >= 0; i--)
                stack.Push((currentNode.Child, currentNode.Child.Children[i]));
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
        using var stackPool = new PooledResource<Stack<(IEntityNode? Parent, IEntityNode Node)>>(entityTreePools.ParentedStackPool);
        using var assetsPool = new PooledResource<List<(IAssetInstance? Parent, IAssetInstance Child)>>(entityTreePools.ParentedListPool);
        Stack<(IEntityNode? Parent, IEntityNode Node)> stack = stackPool.Item;
        List<(IAssetInstance? Parent, IAssetInstance Child)> assets = assetsPool.Item;

        stack.Push((null, Root));
        while (stack.Count > 0) {
            (IEntityNode? parent, IEntityNode node) = stack.Pop();
            assets.Add((parent?.Value, node.Value));

            for (int i = node.Children.Count - 1; i >= 0; i--)
                stack.Push((node, node.Children[i]));
        }

        for (int i = assets.Count - 1; i >= 0; i--)
            yield return assets[i];
    }
    #endregion
    
    
    
    
}
