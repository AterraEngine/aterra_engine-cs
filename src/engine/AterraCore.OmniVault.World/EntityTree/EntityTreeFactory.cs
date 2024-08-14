// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Common.Types.Nexities;
using AterraCore.Contracts.Nexities.Components;
using AterraCore.Contracts.Nexities.Levels;
using AterraCore.Contracts.OmniVault.Assets;

namespace AterraCore.OmniVault.World.EntityTree;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------

public class EntityTreeFactory(IAssetInstanceAtlas instanceAtlas) {
    public EntityTree CreateFromLevel(Ulid rootInstanceId) {
        var tree = new EntityTree(PopulateNodes(lrootInstanceId));
        
        return tree;
    }
    private EntityNode PopulateNodes(Ulid instanceId) {
        if (!instanceAtlas.TryGet(instanceId, out IAssetInstance? instance)) throw new ArgumentException();

        EntityNode node = new(instance);
        if (instance is IHasDirectChildren hasDirectChildren) {
            foreach (Ulid childId in hasDirectChildren.ChildrenIDs.Children) {
                node.Children.Add(PopulateNodes(childId));
            }
        }
}
