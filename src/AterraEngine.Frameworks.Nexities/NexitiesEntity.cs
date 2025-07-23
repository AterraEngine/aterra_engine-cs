// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Buffers;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using AterraEngine.Frameworks.Nexities.Pools;

namespace AterraEngine.Frameworks.Nexities;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class NexitiesEntity<TEntity>(int initialComponentCapacity = 0) : INexitiesEntity
    where TEntity : class, INexitiesEntity, new()
{
    private INexitiesComponent[] Components { get; set; } = ArrayPool<INexitiesComponent>.Shared.Rent(initialComponentCapacity);
    private uint ComponentCount { get; set; }
    
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    private void ResizeComponentsArrayIfNeeded(int newSize) {
        if (newSize <= Components.Length) return;
        
        var newComponents = ArrayPool<INexitiesComponent>.Shared.Rent(newSize);
        Components.CopyTo(newComponents, 0);
        
        ArrayPool<INexitiesComponent>.Shared.Return(Components);
        Components = newComponents;
    }

    protected void SetComponent(INexitiesComponent component, int index) {
        ResizeComponentsArrayIfNeeded(index + 1);
        Components[index] = component;
        ComponentCount++;
    }
    
    protected void SetComponent<TComponent>(int index) where TComponent : class, INexitiesComponent, new() 
        => SetComponent(new TComponent(), index);
    
    protected TComponent GetComponent<TComponent>(int index) where TComponent : INexitiesComponent {
        if (index >= ComponentCount) throw new IndexOutOfRangeException();
        return Unsafe.As<INexitiesComponent, TComponent>(ref Components[index]);
    }

    protected bool TryGetComponent<TComponent>(int index, [NotNullWhen(true)] out TComponent? component)
        where TComponent : INexitiesComponent {
        component = default;
        if (index + 1 >= ComponentCount) return false;
        if (Components[index] is not TComponent componentCasted) return false;
        component = componentCasted;
        return true;
    }

    // -----------------------------------------------------------------------------------------------------------------
    // Cleanup Methods
    // -----------------------------------------------------------------------------------------------------------------
    public virtual bool TryReset() {
        ComponentCount = 0;
        
        for (var i = 0; i < ComponentCount; i++) {
            INexitiesComponent component = Components[i];
            component.ReturnToPool();
        }
        
        ArrayPool<INexitiesComponent>.Shared.Return(Components, true);
        Components = ArrayPool<INexitiesComponent>.Shared.Rent(initialComponentCapacity);
        
        return true;
    }

    public virtual void ReturnToPool() => EntityPool<TEntity>.Shared.Return(Unsafe.As<TEntity>(this));
}