// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Contracts.OmniVault.Assets;
using AterraCore.Contracts.OmniVault.World.EntityTree;
using JetBrains.Annotations;
using Microsoft.Extensions.ObjectPool;

namespace AterraCore.OmniVault.World.EntityTree.Pools;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[UsedImplicitly]
public class EntityTreePools : IEntityTreePools {
    private const int InitialCapacity = 1024;
    
    public ObjectPool<Queue<IEntityNode>> QueuePool {get;} =
        new DefaultObjectPoolProvider().Create(new QueuePooledObjectPolicy(InitialCapacity));
    public ObjectPool<Stack<IEntityNode>> StackPool {get;} =
        new DefaultObjectPoolProvider().Create(new StackPooledObjectPolicy(InitialCapacity));
    public ObjectPool<List<IAssetInstance>> ListPool {get;} =
        new DefaultObjectPoolProvider().Create(new ListPooledObjectPolicy(InitialCapacity));
    public ObjectPool<Stack<(IEntityNode? Parent, IEntityNode Node)>> ParentedStackPool {get;} =
        new DefaultObjectPoolProvider().Create(new ParentedStackPooledObjectPolicy(InitialCapacity));
    public ObjectPool<List<(IAssetInstance? Parent, IAssetInstance Child)>> ParentedListPool {get;} =
        new DefaultObjectPoolProvider().Create(new  ParentedListPooledObjectPolicy(InitialCapacity));
}
