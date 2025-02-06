// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Frameworks.Omnia;
using CodeOfChaos.Types;
using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;

namespace AterraEngine.Frameworks.Nexities;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public abstract class NexitiesEntity : OmniaAsset, INexitiesEntity {
    public TypedValueStore<Type> Components { get; } = new();
    
    
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public bool TryGetComponent<TComponent>([NotNullWhen(true)] out TComponent? component) where TComponent : INexitiesComponent {
        return Components.TryGetValue(typeof(TComponent), out component);
    }

    public bool TryRegisterComponent<TComponent>(TComponent component) where TComponent : INexitiesComponent {
        return Components.TryAdd(typeof(TComponent), component);
    }

    public override void Cleanup() {
        base.Cleanup();
        
        // TODO inject this or go through "EngineServices"
        var library = new OmniaLibrary();
        
        foreach (INexitiesComponent component in Components) {
            library.ReturnAsset(component);
        }
        
    }

}
