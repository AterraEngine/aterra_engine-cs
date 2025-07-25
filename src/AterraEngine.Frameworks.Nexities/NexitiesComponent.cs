// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

using System.Runtime.CompilerServices;
using AterraEngine.Frameworks.Nexities.Pools;

namespace AterraEngine.Frameworks.Nexities;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public abstract class NexitiesComponent<TComponent> : INexitiesComponent where TComponent : class, INexitiesComponent, new() {
    public Guid Id { get; private set; } = Guid.CreateVersion7();
    
    // -----------------------------------------------------------------------------------------------------------------
    // Cleanup Methods
    // -----------------------------------------------------------------------------------------------------------------
    public virtual bool TryReset() {
        Id = Guid.CreateVersion7();
        return true;
    }
    
    public virtual void ReturnToPool() {
        ComponentPool<TComponent>.Shared.Return(Unsafe.As<TComponent>(this));
    }
}