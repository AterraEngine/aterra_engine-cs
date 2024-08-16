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
public readonly struct EntityNodeTree(IEntityNode root, IEntityTreePools entityTreePools) : IEntityNodeTree {
    private IEntityNode Root { get; } = root;
    
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------=
    #region Flat | root -> child -> n-grandchildren
    public IEnumerable<IAssetInstance> GetAsFlat() {
        using var stackPool = new PooledObject<Stack<IEntityNode>>(entityTreePools.StackPool);
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
    public IEnumerable<IAssetInstance> GetAsFlatReverse() {
        using var queuePool = new PooledObject<Queue<IEntityNode>>(entityTreePools.QueuePool);
        using var assetsPool = new PooledObject<List<IAssetInstance>>(entityTreePools.ListPool);
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
    public IEnumerable<(IAssetInstance? Parent,IAssetInstance Child)> GetAsFlatWithParent() {
        using var stackPool = new PooledObject<Stack<(IEntityNode? Parent, IEntityNode Child)>>(entityTreePools.ParentedStackPool);
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
    public IEnumerable<(IAssetInstance? Parent,IAssetInstance Child)> GetAsFlatReverseWithParent() {
        using var stackPool = new PooledObject<Stack<(IEntityNode? Parent, IEntityNode Node)>>(entityTreePools.ParentedStackPool);
        using var assetsPool = new PooledObject<List<(IAssetInstance? Parent, IAssetInstance Child)>>(entityTreePools.ParentedListPool);
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
