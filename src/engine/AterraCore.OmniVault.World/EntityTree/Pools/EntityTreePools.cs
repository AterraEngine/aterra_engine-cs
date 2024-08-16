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
    private readonly DefaultObjectPoolProvider _objectPoolProvider = new();

    private ObjectPool<Stack<(IEntityNode ParentNode, Ulid InstanceId)>>? _factoryStack;
    public ObjectPool<Stack<(IEntityNode ParentNode, Ulid InstanceId)>> FactoryStack => 
        _factoryStack ??= _objectPoolProvider.Create(new FactoryStackPooledObjectPolicy(InitialCapacity));
        
    private ObjectPool<Queue<IEntityNode>>? _queuePool;
    public ObjectPool<Queue<IEntityNode>> QueuePool => 
        _queuePool ??= _objectPoolProvider.Create(new QueuePooledObjectPolicy(InitialCapacity));
        
    private ObjectPool<Stack<IEntityNode>>? _stackPool;
    public ObjectPool<Stack<IEntityNode>> StackPool => 
        _stackPool ??= _objectPoolProvider.Create(new StackPooledObjectPolicy(InitialCapacity));
        
    private ObjectPool<List<IAssetInstance>>? _listPool;
    public ObjectPool<List<IAssetInstance>> ListPool => 
        _listPool ??= _objectPoolProvider.Create(new ListPooledObjectPolicy(InitialCapacity));
        
    private ObjectPool<Stack<(IEntityNode? Parent, IEntityNode Node)>>? _parentedStackPool;
    public ObjectPool<Stack<(IEntityNode? Parent, IEntityNode Node)>> ParentedStackPool => 
        _parentedStackPool ??= _objectPoolProvider.Create(new ParentedStackPooledObjectPolicy(InitialCapacity));
        
    private ObjectPool<List<(IAssetInstance? Parent, IAssetInstance Child)>>? _parentedListPool;
    public ObjectPool<List<(IAssetInstance? Parent, IAssetInstance Child)>> ParentedListPool => 
        _parentedListPool ??= _objectPoolProvider.Create(new ParentedListPooledObjectPolicy(InitialCapacity));
}
