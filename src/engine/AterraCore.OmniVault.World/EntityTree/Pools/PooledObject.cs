﻿// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using Microsoft.Extensions.ObjectPool;

namespace AterraCore.OmniVault.World.EntityTree.Pools;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------

public readonly struct PooledObject<T>(ObjectPool<T> pool) : IDisposable where T : class {
    public T Item { get; } = pool.Get();
    public void Dispose() => pool.Return(Item);

}
