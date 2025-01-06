// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Frameworks.Omnia;
using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;

namespace AterraEngine.Frameworks.Nexities;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public abstract class NexitiesEntity : OmniaAsset, INexitiesEntity {
    public ConcurrentDictionary<OmniaId, INexitiesComponent> Components { get; } = new();

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public bool TryGetComponent<TComponent>(OmniaId omniaId, [NotNullWhen(true)] out TComponent? exampleComponent) where TComponent : INexitiesComponent {
        exampleComponent = default;

        if (!Components.TryGetValue(omniaId, out INexitiesComponent? component) || component is not TComponent typedComponent) return false;

        exampleComponent = typedComponent;
        return true;
    }

    public bool TryRegisterComponent<TComponent>(TComponent component) where TComponent : INexitiesComponent {
        return Components.TryAdd(component.OmniaId, component);
    }

    public override void Cleanup() {
        base.Cleanup();
        
        // TODO inject this or go through "EngineServices"
        var library = new OmniaLibrary();
        
        foreach (INexitiesComponent component in Components.Values) {
            library.ReturnAsset(component);
        }
        
    }

}
