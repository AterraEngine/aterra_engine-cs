﻿// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Contracts.DI;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace AterraCore.DI;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
/// <summary>
///     The Dependency Injection Builder for the Aterra Engine.
/// </summary>
/// <param name="logger"></param>
/// <param name="collection">A pre-made service collection.</param>
public class EngineServiceBuilder(ILogger logger, IServiceCollection? collection = null) : IEngineServiceBuilder {
    private ILogger Logger { get; } = logger.ForContext<EngineServiceBuilder>();

    /// <summary>
    ///     Represents a builder for configuring and building the service collection in the AterraCore engine.
    /// </summary>
    public IServiceCollection ServiceCollection { get; } = collection ?? new ServiceCollection();

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    /// <summary>
    ///     Assigns a <see cref="ServiceDescriptor" /> to the <see cref="IServiceCollection" /> of the
    ///     <see cref="EngineServiceBuilder" />.
    /// </summary>
    /// <param name="serviceDescriptor">The <see cref="ServiceDescriptor" /> to assign.</param>
    public void AssignFromServiceDescriptor(ServiceDescriptor serviceDescriptor) {
        ServiceCollection.Add(serviceDescriptor);
        Logger.Debug(
            "Type {Type} assigned to {Imp}",
            serviceDescriptor.ServiceType,
            serviceDescriptor.ImplementationType ?? serviceDescriptor.ImplementationInstance ?? serviceDescriptor.ServiceType
        );
    }

    /// <summary>
    ///     Assigns multiple <see cref="ServiceDescriptor" /> objects to the <see cref="IServiceCollection" /> of the
    ///     <see cref="EngineServiceBuilder" />.
    /// </summary>
    /// <param name="services">The collection of <see cref="ServiceDescriptor" /> objects.</param>
    public void AssignFromServiceDescriptors(IEnumerable<ServiceDescriptor> services) {
        foreach (ServiceDescriptor serviceDescriptor in services) {
            AssignFromServiceDescriptor(serviceDescriptor);
        }
    }

    /// <summary>
    ///     Finishes building the engine service builder by building all services and logging the completion.
    /// </summary>
    public void FinishBuilding() {
        // END of factory,
        //      build all services
        EngineServices.BuildServiceProvider(ServiceCollection);
        Logger.Information("Dependency Container Built");
    }
}
