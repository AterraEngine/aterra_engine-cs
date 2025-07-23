// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.ObjectPool;

namespace AterraEngine.Frameworks.Nexities;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface INexitiesEntity : IResettable {

    void SetComponent(INexitiesComponent component, int index);
    void SetComponent<TComponent>(int index) where TComponent : class, INexitiesComponent, new();
    TComponent GetComponent<TComponent>(int index) where TComponent : INexitiesComponent;
    bool TryGetComponent<TComponent>(int index, [NotNullWhen(true)] out TComponent? component) where TComponent : INexitiesComponent;
    ReadOnlySpan<INexitiesComponent> GetComponents();
    
    void ReturnToPool();
}