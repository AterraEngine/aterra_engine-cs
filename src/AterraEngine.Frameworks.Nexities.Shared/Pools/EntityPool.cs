// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

using CodeOfChaos.Extensions.ObjectPool;
using Microsoft.Extensions.ObjectPool;

namespace AterraEngine.Frameworks.Nexities.Pools;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public static class EntityPool<TEntity> where TEntity : class, INexitiesEntity, new() {
    public static ObjectPool<TEntity> Shared { get; } = new DefaultObjectPool<TEntity>(new ResettablePoolPolicy<TEntity>());
}