// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

using Microsoft.Extensions.DependencyInjection;

namespace AterraCore.Contracts.DI;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IEngineServiceBuilder {
    public IServiceCollection ServiceCollection { get; }
    
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public void AssignDefaultServices(IEnumerable<ServiceDescriptor> services);
    public void AssignStaticServices(IEnumerable<ServiceDescriptor> services);
    public void AssignServicesFromPlugins(IEnumerable<ServiceDescriptor> services);
    public void FinishBuilding();
}