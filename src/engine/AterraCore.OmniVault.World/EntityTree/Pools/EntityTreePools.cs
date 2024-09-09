﻿// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Attributes;
using AterraCore.Contracts.OmniVault.Assets;
using AterraCore.Contracts.OmniVault.World.EntityTree;
using JetBrains.Annotations;
using Microsoft.Extensions.ObjectPool;

namespace AterraCore.OmniVault.World.EntityTree.Pools;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[UsedImplicitly]
[Singleton<IEntityTreePools>]
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

    private ObjectPool<Queue<IAssetInstance>>? _queueAssetInstancePool;
    public ObjectPool<Queue<IAssetInstance>> QueueAssetInstancePool =>
        _queueAssetInstancePool ??= _objectPoolProvider.Create(new QueueAssetInstancePooledObjectPolicy(InitialCapacity));

    private ObjectPool<Stack<(IEntityNode? Parent, IEntityNode Node)>>? _parentedStackPool;
    public ObjectPool<Stack<(IEntityNode? Parent, IEntityNode Node)>> ParentedStackPool =>
        _parentedStackPool ??= _objectPoolProvider.Create(new ParentedStackPooledObjectPolicy(InitialCapacity));

    private ObjectPool<Stack<(IAssetInstance? Parent, IAssetInstance Child)>>? _parentedStackAssetInstanceListPool;
    public ObjectPool<Stack<(IAssetInstance? Parent, IAssetInstance Node)>> ParentedStackAssetInstancePool =>
        _parentedStackAssetInstanceListPool ??= _objectPoolProvider.Create(new ParentedStackAssetInstancePooledObjectPolicy(InitialCapacity));
}
