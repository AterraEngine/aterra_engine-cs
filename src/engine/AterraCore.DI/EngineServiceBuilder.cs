// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

using AterraCore.Common;
using AterraCore.Contracts.DI;
using AterraCore.Loggers;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace AterraCore.DI;

// ---------------------------------------------------------------------------------------------------------------------
// Support Code
// ---------------------------------------------------------------------------------------------------------------------


// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class EngineServiceBuilder(ILogger logger) : IEngineServiceBuilder {
    public IServiceCollection ServiceCollection { get; } = new ServiceCollection();
    
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public void AssignDefaultServices() {
        // Add DEFAULT services here
        //      These services can be overriden
        // ---
        
        ServiceCollection.AddSingleton(EngineLogger.CreateLogger());
        
        // ---
    }

    public void AssignStaticServices(IEnumerable<StaticService> services) {
        // Add STATIC Services here
        //      These services cannot be overriden
        // ---
        foreach (StaticService service in services) {
            if (service.Type != ServiceType.Singleton) continue;
            
            ServiceCollection.AddSingleton(service.Interface, service.Implementation);
            logger.Debug("Assigned {Interface} to {Implementation} as a Singleton", service.Interface.FullName, service.Implementation.FullName);

            // TODO add the other implementations as well
        }
        
        // ---
    }

    public void FinishBuilding() {
        // END of factory,
        //      build all services
        EngineServices.BuildServiceProvider(ServiceCollection);
    }
}