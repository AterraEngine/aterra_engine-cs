// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Contracts.OmniVault.Assets;

namespace AterraCore.Contracts.OmniVault.World.EntityTree;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------

public interface IEntityNodeTree {
    public IEnumerable<IAssetInstance> GetAsFlat();
    public IEnumerable<IAssetInstance> GetAsFlatReverse();

    public IEnumerable<(IAssetInstance? Parent, IAssetInstance Child)> GetAsFlatWithParent();
    public IEnumerable<(IAssetInstance? Parent, IAssetInstance Child)> GetAsFlatReverseWithParent();
}
