// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Contracts.OmniVault.Assets;
using Microsoft.Extensions.ObjectPool;

namespace AterraCore.Contracts.OmniVault.World.EntityTree;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IEntityTreePools {
    ObjectPool<Queue<IEntityNode>> QueuePool {get;}
    ObjectPool<Stack<IEntityNode>> StackPool {get;}
    ObjectPool<List<IAssetInstance>> ListPool {get;}
    ObjectPool<Stack<(IEntityNode? Parent, IEntityNode Node)>> ParentedStackPool {get;}
    ObjectPool<List<(IAssetInstance? Parent, IAssetInstance Child)>> ParentedListPool {get;}
}
