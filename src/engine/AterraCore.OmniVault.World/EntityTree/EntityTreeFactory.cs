// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Common.Types;
using AterraCore.Contracts.Nexities.Entities.QuickHands;
using AterraCore.Contracts.OmniVault.Assets;
using AterraCore.Contracts.OmniVault.World.EntityTree;
using JetBrains.Annotations;

namespace AterraCore.OmniVault.World.EntityTree;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[UsedImplicitly]
public class EntityTreeFactory(IAssetInstanceAtlas instanceAtlas, IEntityTreePools entityTreePools) : IEntityTreeFactory {
    public IEntityNodeTree CreateFromRootId(Ulid rootInstanceId) => new EntityNodeTree(PopulateNodes(rootInstanceId), entityTreePools);
    public IEntityNodeTree CreateEmpty() => new EntityNodeTree(new EntityNode(), entityTreePools);

    /// <summary>
    /// Populates the nodes in the entity node tree starting from the given root instance ID.
    /// </summary>
    /// <param name="rootInstanceId">The ULID of the root instance.</param>
    /// <returns>The root entity node of the populated tree.</returns>
    private EntityNode PopulateNodes(Ulid rootInstanceId) {
        if (!instanceAtlas.TryGet(rootInstanceId, out IAssetInstance? instance)) throw new ArgumentException("Node was not a IAssetInstance");

        var rootNode = new EntityNode(instance);
        if (instance is not IHasDirectChildren) return rootNode;

        using var stackPool = new PooledResource<Stack<(IEntityNode ParentNode, Ulid InstanceId)>>(entityTreePools.FactoryStack);
        Stack<(IEntityNode ParentNode, Ulid InstanceId)> stack = stackPool.Item;

        stack.Push((rootNode, rootInstanceId));
        while (stack.TryPop(out (IEntityNode ParentNode, Ulid InstanceId) node)) {
            (IEntityNode parentNode, Ulid ulid) = node;

            if (!instanceAtlas.TryGet(ulid, out IAssetInstance? childInstance)) continue;// the asset wasn't managed by AterraEngine
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
