// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Contracts.DI;
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
    public void AssignServiceDescriptors(IEnumerable<ServiceDescriptor> services) {
        foreach (ServiceDescriptor serviceDescriptor in services) {
            ServiceCollection.Add(serviceDescriptor);
            logger.Information(
                "Type {Type} assigned to {Imp}", 
                serviceDescriptor.ServiceType, 
                serviceDescriptor.ImplementationType
            );
        }
    }
    
    public void FinishBuilding() {
        // END of factory,
        //      build all services
        EngineServices.BuildServiceProvider(ServiceCollection);
        logger.Information("Dependency Container Built");
    }
}