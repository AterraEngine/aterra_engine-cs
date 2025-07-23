// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Extensions.ObjectPool;
using Microsoft.Extensions.ObjectPool;

namespace AterraEngine.Frameworks.Nexities.Pools;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public static class ComponentPool<TComponent> where TComponent : class, INexitiesComponent, new() {
    public static ObjectPool<TComponent> Shared { get; } = new DefaultObjectPool<TComponent>(new ResettablePoolPolicy<TComponent>(), 256);
}