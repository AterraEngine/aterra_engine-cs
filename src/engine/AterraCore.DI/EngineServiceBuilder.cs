// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Contracts.DI;
using Extensions;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace AterraCore.DI;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class EngineServiceBuilder(ILogger logger) : IEngineServiceBuilder {
    public IServiceCollection ServiceCollection { get; } = new ServiceCollection();
    
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public void AssignFromServiceDescriptor(ServiceDescriptor serviceDescriptor) {
        ServiceCollection.Add(serviceDescriptor);
        logger.Information(
            "Type {Type} assigned to {Imp}", 
            serviceDescriptor.ServiceType, 
            serviceDescriptor.ImplementationType
        );
    }
    public void AssignFromServiceDescriptors(IEnumerable<ServiceDescriptor> services) => services.IterateOver(AssignFromServiceDescriptor);

    public void FinishBuilding() {
        // END of factory,
        //      build all services
        EngineServices.BuildServiceProvider(ServiceCollection);
        logger.Information("Dependency Container Built");
    }
}