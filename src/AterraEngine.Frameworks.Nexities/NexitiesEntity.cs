// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Buffers;
using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using AterraEngine.Contracts;
using AterraEngine.Frameworks.Nexities.Pools;
using AterraEngine.Shared;

namespace AterraEngine.Frameworks.Nexities;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public abstract class NexitiesEntity<TEntity>(int initialComponentCapacity = 0) : INexitiesEntity
    where TEntity : class, INexitiesEntity, new() 
{
    private INexitiesComponent[] Components { get; set; } = ArrayPool<INexitiesComponent>.Shared.Rent(initialComponentCapacity);
    private ConcurrentDictionary<Guid, int> ComponentIdMap { get; } = new();
    
    public int ComponentCount { get; private set => field = Math.Max(0, value); }

    protected static INexitiesComponentProvider ComponentProvider { get; } = IEngine.Instance.GetRequiredService<INexitiesComponentProvider>();

    // -----------------------------------------------------------------------------------------------------------------
    // Constructors
    // -----------------------------------------------------------------------------------------------------------------
    protected abstract void PopulateComponents();
    
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    private void ResizeComponentsArrayIfNeeded(int newSize) {
        if (newSize <= Components.Length) return;
        
        var newComponents = ArrayPool<INexitiesComponent>.Shared.Rent(ComponentCount * 2);
        Components.CopyTo(newComponents, 0);
        
        ArrayPool<INexitiesComponent>.Shared.Return(Components);
        Components = newComponents;
    }

    public void SetComponent(INexitiesComponent component, int index) {
        ResizeComponentsArrayIfNeeded(index + 1);
        Components[index] = component;
        ComponentIdMap.AddOrUpdate(component.Id, index, (_, _) => index);
        ComponentCount++;
    }
    
    public TComponent GetComponent<TComponent>(int index) where TComponent : INexitiesComponent {
        if (index >= ComponentCount) throw new IndexOutOfRangeException();
        return Unsafe.As<INexitiesComponent, TComponent>(ref Components[index]);
    }

    public bool TryGetComponent<TComponent>(int index, [NotNullWhen(true)] out TComponent? component)
        where TComponent : INexitiesComponent {
        component = default;
        if (index + 1 >= ComponentCount) return false;
        if (Components[index] is not TComponent componentCasted) return false;
        component = componentCasted;
        return true;
    }
    
    public ReadOnlySpan<INexitiesComponent> GetComponents() 
        => Components.AsSpan(0, ComponentCount);

    // -----------------------------------------------------------------------------------------------------------------
    // Cleanup Methods
    // -----------------------------------------------------------------------------------------------------------------
    public virtual bool TryReset() {
        int oldComponentCount = ComponentCount;
        ComponentCount = 0;
        
        for (var i = 0; i < ComponentCount; i++) {
            ComponentProvider.ReturnComponent(Components[i]);
            Components[i] = null!;
        }
        
        ArrayPool<INexitiesComponent>.Shared.Return(Components);
        Components = ArrayPool<INexitiesComponent>.Shared.Rent(oldComponentCount);

        PopulateComponents();
        
        return true;
    }

    public virtual void ReturnToPool() => EntityPool<TEntity>.Shared.Return(Unsafe.As<TEntity>(this));
}