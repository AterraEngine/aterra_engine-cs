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
    ObjectPool<Stack<(IEntityNode ParentNode, Ulid InstanceId)>> FactoryStack { get;}
    
    ObjectPool<Queue<IEntityNode>> QueuePool {get;}
    ObjectPool<Stack<IEntityNode>> StackPool {get;}
    ObjectPool<Queue<IAssetInstance>> QueueAssetInstancePool {get;}
    ObjectPool<Stack<(IEntityNode? Parent, IEntityNode Node)>> ParentedStackPool {get;}
    ObjectPool<Stack<(IAssetInstance? Parent, IAssetInstance Node)>> ParentedStackAssetInstancePool {get;}
}
