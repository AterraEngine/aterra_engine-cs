// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Contracts.OmniVault.Assets;

namespace AterraCore.Contracts.OmniVault.World.EntityTree;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------

public interface IEntityNodeTree {
    IEnumerable<IAssetInstance> GetAsFlat();
    IEnumerable<IAssetInstance> GetAsFlatReverse();
    IEnumerable<(IAssetInstance? Parent, IAssetInstance Child)> GetAsFlatWithParent();
    IEnumerable<(IAssetInstance? Parent, IAssetInstance Child)> GetAsFlatReverseWithParent();

    IEnumerable<IEntityNode> TraverseDepthFirst();
    IEnumerable<IEntityNode> TraverseBreadthFirst();
}
