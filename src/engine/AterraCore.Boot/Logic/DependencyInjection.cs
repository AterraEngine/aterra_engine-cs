// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Contracts.Boot;
using Extensions;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using static Extensions.ServiceDescriptorExtension;

namespace AterraCore.Boot.Logic;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------

public static class DependencyInjection {
    public static IEngineConfiguration AddDefaultServices(this IEngineConfiguration configuration, params ServiceDescriptor[] serviceDescriptors) {
        // Systems which may be overriden
        configuration.ServicesDefault.AddLastRepeated(serviceDescriptors);
        return configuration;
    }
    
    public static IEngineConfiguration AddStaticServices(this IEngineConfiguration configuration, params ServiceDescriptor[] serviceDescriptors) {
        // services which may not be overriden
        configuration.ServicesStatic.AddLastRepeated(serviceDescriptors);
        return configuration;
    }
    
    public static IEngineConfiguration BuildDependencyInjectionContainer(this IEngineConfiguration configuration) {
        ILogger logger = configuration.StartupLog;
        
        logger.Information("Starting to assign Default Services ...");
        // Add ILogger before anything else, to be safe.
        configuration.ServicesDefault.AddFirst(NewServiceDescriptor<ILogger>(configuration.EngineLoggerCallback()));
        configuration.EngineServiceBuilder.AssignFromServiceDescriptors(configuration.ServicesDefault);
        logger.Information("Assigned Default Services");

        logger.Information("Starting to assign Static Services ...");
        HashSet<Type> staticServices = [];
        IEnumerable<Type> query = configuration.ServicesStatic
            .Select(serviceDescriptor => serviceDescriptor.ServiceType)
            .Where(type => !staticServices.Add(type));
        
        foreach (Type type in query) {
            logger.Error("StaticType of {type} has already been stored", type);
        }
        
        configuration.EngineServiceBuilder.AssignFromServiceDescriptors(configuration.ServicesStatic);
        logger.Information("Assigned Static Services");
        
        configuration.EngineServiceBuilder.FinishBuilding();
        return configuration;

    }
}
