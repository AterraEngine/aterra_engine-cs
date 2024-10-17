// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Contracts.OmniVault.World.EntityTree;
using Microsoft.Extensions.ObjectPool;

namespace AterraEngine.EntityTree.Pools;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class QueuePooledObjectPolicy(int initialCapacity) : PooledObjectPolicy<Queue<IEntityNode>> {
    public override Queue<IEntityNode> Create() => new(initialCapacity);
    public override bool Return(Queue<IEntityNode> obj) {
        obj.Clear();
        return true;
    }
}
