// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Contracts.Nexities.Components;
using AterraCore.Contracts.Nexities.Levels;
using AterraCore.Contracts.OmniVault.Assets;
using AterraCore.Contracts.OmniVault.World.EntityTree;
using AterraCore.OmniVault.World.EntityTree.Pools;
using JetBrains.Annotations;

namespace AterraCore.OmniVault.World.EntityTree;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[UsedImplicitly]
public class EntityTreeFactory(IAssetInstanceAtlas instanceAtlas, IEntityTreePools entityTreePools) : IEntityTreeFactory {
    public IEntityNodeTree CreateFromRootId(Ulid rootInstanceId) => new EntityNodeTree(PopulateNodes(rootInstanceId),entityTreePools);
    public IEntityNodeTree CreateEmpty() => new EntityNodeTree(new EntityNode(), entityTreePools);

    private EntityNode PopulateNodes(Ulid instanceId) {
        if (!instanceAtlas.TryGet(instanceId, out IAssetInstance? instance)) throw new ArgumentException("Node was not a IAssetInstance");

        var rootNode = new EntityNode(instance);
        if (instance is not IHasDirectChildren) return rootNode;
        
        using var stackPool = new PooledResource<Stack<(IEntityNode ParentNode, Ulid InstanceId)>>(entityTreePools.FactoryStack);
        Stack<(IEntityNode ParentNode, Ulid InstanceId)> stack = stackPool.Item;
        
        stack.Push((rootNode, instanceId));
        while (stack.TryPop(out (IEntityNode ParentNode, Ulid InstanceId) node)) {
            (IEntityNode parentNode, Ulid ulid) = node;
            if (!instanceAtlas.TryGet(ulid, out IAssetInstance? childInstance)) continue; // the asset wasn't managed by AtteraEngine
            var currentNode = new EntityNode(childInstance);
            parentNode.Children.Add(currentNode);
            
            if (childInstance is not IHasDirectChildren hasDirectChildren) continue;
            foreach (Ulid grandChildId in hasDirectChildren.ChildrenIDs.Children) {
               stack.Push((currentNode, grandChildId));
            }
        }

        return rootNode;
    }
}
