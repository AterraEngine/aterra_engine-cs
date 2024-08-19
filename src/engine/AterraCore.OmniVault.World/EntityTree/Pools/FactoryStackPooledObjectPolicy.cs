// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Contracts.OmniVault.World.EntityTree;
using Microsoft.Extensions.ObjectPool;

namespace AterraCore.OmniVault.World.EntityTree.Pools;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class FactoryStackPooledObjectPolicy(int initialCapacity) : PooledObjectPolicy<Stack<(IEntityNode ParentNode, Ulid InstanceId)>> {
    public override Stack<(IEntityNode ParentNode, Ulid InstanceId)> Create() => new(initialCapacity);
    public override bool Return(Stack<(IEntityNode ParentNode, Ulid InstanceId)> obj) {
        obj.Clear();
        return true;
    }
}
