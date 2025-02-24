// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Frameworks.Omnia;
using System.Diagnostics.CodeAnalysis;

namespace AterraEngine.Frameworks.Nexities;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public abstract class NexitiesEntity : OmniaAsset, INexitiesEntity {
    public TypedValueStore<Type> Components { get; } = new();

    public override void Cleanup() {
        base.Cleanup();

        // TODO inject this or go through "EngineServices"
        var library = new OmniaLibrary();

        foreach (INexitiesComponent component in Components) {
            library.ReturnAsset(component);
        }

    }


    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public bool TryGetComponent<TComponent>([NotNullWhen(true)] out TComponent? component) where TComponent : INexitiesComponent => Components.TryGetValue(typeof(TComponent), out component);

    public bool TryRegisterComponent<TComponent>(TComponent component) where TComponent : INexitiesComponent => Components.TryAdd(typeof(TComponent), component);
}
