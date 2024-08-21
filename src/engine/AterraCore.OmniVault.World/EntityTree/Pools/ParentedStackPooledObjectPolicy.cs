// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Contracts.OmniVault.World.EntityTree;
using Microsoft.Extensions.ObjectPool;

namespace AterraCore.OmniVault.World.EntityTree.Pools;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class ParentedStackPooledObjectPolicy(int initialCapacity) : PooledObjectPolicy<Stack<(IEntityNode? Parent, IEntityNode Node)>> {
    public override Stack<(IEntityNode? Parent, IEntityNode Node)> Create() => new(initialCapacity);
    public override bool Return(Stack<(IEntityNode? Parent, IEntityNode Node)> obj) {
        obj.Clear();
        return true;
    }
}
