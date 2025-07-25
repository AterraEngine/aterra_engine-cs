// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

using System.Collections.Concurrent;
using System.Runtime.CompilerServices;
using AterraEngine.Contracts;
using AterraEngine.Frameworks.Nexities.Pools;
using AterraEngine.Shared;
using CodeOfChaos.Extensions.DependencyInjection;

namespace AterraEngine.Frameworks.Nexities;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[InjectableSingleton<INexitiesComponentProvider>]
public class NexitiesComponentProvider(IEngine engine) : INexitiesComponentProvider {
    private ConcurrentDictionary<Guid, object> CachedComponentsMap { get; } = new();
    private ConcurrentDictionary<Type, Guid> SingletonIdMap { get; } = new();

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public TComponent GetComponent<TComponent>(Guid guid) where TComponent : class, INexitiesComponent, new() {
        if (guid != Guid.Empty && CachedComponentsMap.TryGetValue(guid, out object? specificComponent)) return Unsafe.As<TComponent>(specificComponent);
        return GetComponent<TComponent>();
    }
    
    public TComponent GetComponent<TComponent>() where TComponent : class, INexitiesComponent, new() {
        if (SingletonIdMap.TryGetValue(typeof(TComponent), out Guid guid) && CachedComponentsMap.TryGetValue(guid, out object? component)) return Unsafe.As<TComponent>(component);
        if (engine.GetService<TComponent>() is {} componentInstance) return componentInstance;
        return ComponentPool<TComponent>.Shared.Get();
    }
    
    public void RegisterSingleton<T>(T component) where T : class, INexitiesComponent {
        SingletonIdMap.AddOrUpdate(typeof(T), component.Id, (_, _) => component.Id);
        CachedComponentsMap.AddOrUpdate(component.Id, component, (_, _) => component);
    }

    public void ReturnComponent(in INexitiesComponent component) {
        if (component.Id == Guid.Empty) return; // Has already been cleared
        if (SingletonIdMap.ContainsKey(component.GetType())) return; // Is a singleton
        
        CachedComponentsMap.TryRemove(component.Id, out _);
        component.ReturnToPool();
    }
}