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
    // -----------------------------------------------------------------------------------------------------------------
    // Cleanup Methods
    // -----------------------------------------------------------------------------------------------------------------
    public virtual bool TryReset() => true;
    public virtual void ReturnToPool() => ComponentPool<TComponent>.Shared.Return(Unsafe.As<TComponent>(this));
}