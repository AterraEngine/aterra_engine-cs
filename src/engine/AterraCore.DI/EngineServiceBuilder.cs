// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Contracts.DI;
using AterraCore.Loggers;
using CodeOfChaos.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace AterraCore.DI;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class EngineServiceBuilder(ILogger logger) : IEngineServiceBuilder {
    /// <summary>
    /// Represents a logger for the EngineServiceBuilder section.
    /// </summary>
    private ILogger Logger { get; set; } = logger.ForEngineServiceBuilderContext();
    /// <summary>
    /// Represents a builder for configuring and building the service collection in the AterraCore engine.
    /// </summary>
    public IServiceCollection ServiceCollection { get; } = new ServiceCollection();

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    /// <summary>
    /// Assigns a <see cref="ServiceDescriptor"/> to the <see cref="IServiceCollection"/> of the <see cref="EngineServiceBuilder"/>.
    /// </summary>
    /// <param name="serviceDescriptor">The <see cref="ServiceDescriptor"/> to assign.</param>
    public void AssignFromServiceDescriptor(ServiceDescriptor serviceDescriptor) {
        ServiceCollection.Add(serviceDescriptor);
        Logger.Information(
            "Type {Type} assigned to {Imp}",
            serviceDescriptor.ServiceType,
            serviceDescriptor.ImplementationType
        );
    }
    /// <summary>
    /// Assigns service descriptors to the service collection.
    /// </summary>
    /// <param name="services">The service descriptors.</param>
    public void AssignFromServiceDescriptors(IEnumerable<ServiceDescriptor> services) =>
        services.IterateOver(AssignFromServiceDescriptor);

    /// <summary>
    /// Finishes building the engine service builder by building all services and logging the completion.
    /// </summary>
    public void FinishBuilding() {
        // END of factory,
        //      build all services
        EngineServices.BuildServiceProvider(ServiceCollection);
        Logger.Information("Dependency Container Built");
    }
}
