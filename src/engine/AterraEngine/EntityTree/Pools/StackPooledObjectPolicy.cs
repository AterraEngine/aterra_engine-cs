// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Contracts.OmniVault.World.EntityTree;
using Microsoft.Extensions.ObjectPool;

namespace AterraEngine.EntityTree.Pools;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class StackPooledObjectPolicy(int initialCapacity) : PooledObjectPolicy<Stack<IEntityNode>> {
    public override Stack<IEntityNode> Create() => new(initialCapacity);
    public override bool Return(Stack<IEntityNode> obj) {
        obj.Clear();
        return true;
    }
}
