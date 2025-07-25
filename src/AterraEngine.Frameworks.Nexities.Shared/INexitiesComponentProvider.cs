// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

namespace AterraEngine.Frameworks.Nexities;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface INexitiesComponentProvider {
    TComponent GetComponent<TComponent>(Guid guid) where TComponent : class, INexitiesComponent, new();
    TComponent GetComponent<TComponent>() where TComponent : class, INexitiesComponent, new();
    void RegisterSingleton<T>(T component) where T : class, INexitiesComponent;

    void ReturnComponent(in INexitiesComponent component);
}