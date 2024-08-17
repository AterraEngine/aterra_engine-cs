// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Contracts.OmniVault.Assets;
using Microsoft.Extensions.ObjectPool;

namespace AterraCore.OmniVault.World.EntityTree.Pools;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class ParentedStackAssetInstancePooledObjectPolicy(int initialCapacity) : PooledObjectPolicy<Stack<(IAssetInstance? Parent, IAssetInstance Child)>> {
    public override Stack<(IAssetInstance? Parent, IAssetInstance Child)> Create() => new(initialCapacity);
    public override bool Return(Stack<(IAssetInstance? Parent, IAssetInstance Child)> obj) {
        obj.Clear();
        return true;
    }
}
