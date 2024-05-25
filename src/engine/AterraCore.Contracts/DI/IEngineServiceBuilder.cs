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
    public void AssignFromServiceDescriptors(IEnumerable<ServiceDescriptor> services);
    public void AssignFromServiceDescriptor(ServiceDescriptor serviceDescriptor);
    public void FinishBuilding();
}