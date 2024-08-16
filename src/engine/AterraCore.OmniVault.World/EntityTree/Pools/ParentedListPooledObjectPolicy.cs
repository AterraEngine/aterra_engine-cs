// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Contracts.OmniVault.Assets;
using Microsoft.Extensions.ObjectPool;

namespace AterraCore.OmniVault.World.EntityTree.Pools;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class ParentedListPooledObjectPolicy(int initialCapacity) : PooledObjectPolicy<List<(IAssetInstance? Parent, IAssetInstance Child)>> {
    public override List<(IAssetInstance? Parent, IAssetInstance Child)> Create() => new(initialCapacity);
    public override bool Return(List<(IAssetInstance? Parent, IAssetInstance Child)> obj) {
        obj.Clear();
        return true;
    }
}
