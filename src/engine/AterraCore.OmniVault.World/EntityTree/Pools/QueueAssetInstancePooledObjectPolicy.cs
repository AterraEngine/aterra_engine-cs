// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Contracts.OmniVault.Assets;
using Microsoft.Extensions.ObjectPool;

namespace AterraCore.OmniVault.World.EntityTree.Pools;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class QueueAssetInstancePooledObjectPolicy(int initialCapacity) : PooledObjectPolicy<Queue<IAssetInstance>> {
    public override Queue<IAssetInstance> Create() => new(initialCapacity);
    public override bool Return(Queue<IAssetInstance> obj) {
        obj.Clear();
        return true;
    }
}
