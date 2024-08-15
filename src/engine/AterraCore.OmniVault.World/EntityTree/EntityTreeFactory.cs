// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Common.Types.Nexities;
using AterraCore.Contracts.Nexities.Components;
using AterraCore.Contracts.Nexities.Levels;
using AterraCore.Contracts.OmniVault.Assets;
using AterraCore.Contracts.OmniVault.World.EntityTree;
using JetBrains.Annotations;

namespace AterraCore.OmniVault.World.EntityTree;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[UsedImplicitly]
public class EntityTreeFactory(IAssetInstanceAtlas instanceAtlas) : IEntityTreeFactory {
    public IEntityNodeTree CreateFromRootId(Ulid rootInstanceId) => new EntityNodeTree(PopulateNodes(rootInstanceId));
    public IEntityNodeTree CreateEmpty() => new EntityNodeTree(new EntityNode());

    private EntityNode PopulateNodes(Ulid instanceId) {
        if (!instanceAtlas.TryGet(instanceId, out IAssetInstance? instance)) throw new ArgumentException("Node was not a IAssetInstance");

        EntityNode node = new(instance);
        if (instance is not IHasDirectChildren hasDirectChildren) return node;

        foreach (Ulid childId in hasDirectChildren.ChildrenIDs.Children) {
            node.Children.Add(PopulateNodes(childId));
        }

        return node;
    }
}
