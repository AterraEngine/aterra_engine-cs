// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Contracts.OmniVault.Assets;
using Microsoft.Extensions.ObjectPool;

namespace AterraCore.OmniVault.World.EntityTree.Pools;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class ListPooledObjectPolicy(int initialCapacity) : PooledObjectPolicy<List<IAssetInstance>> {
    public override List<IAssetInstance> Create() => new(initialCapacity);
    public override bool Return(List<IAssetInstance> obj) {
        obj.Clear();
        return true;
    }
}
