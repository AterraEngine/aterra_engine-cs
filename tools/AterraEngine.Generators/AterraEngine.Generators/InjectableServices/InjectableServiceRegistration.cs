// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace AterraEngine.Generators.InjectableServices;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public struct InjectableServiceRegistration(string serviceTypeName, string implementationTypeName, string lifetime) {
    public string ServiceTypeName { get; } = serviceTypeName;
    public string ImplementationTypeName { get; } = implementationTypeName;
    public string LifeTime { get; } = lifetime;
    public bool IsEmpty { get; set; } = false;

    public static InjectableServiceRegistration AsEmpty() => new(string.Empty, string.Empty, string.Empty) {
        IsEmpty = true
    };
}
