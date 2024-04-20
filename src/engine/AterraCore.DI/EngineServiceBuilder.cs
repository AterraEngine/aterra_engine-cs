// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

using AterraCore.Contracts.DI;
using AterraCore.Loggers;
using Microsoft.Extensions.DependencyInjection;

namespace AterraCore.DI;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------

public class EngineServiceBuilder : IEngineServiceBuilder {
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

    public void AssignStaticServices() {
        // Add STATIC Services here
        //      These services cannot be overriden
        // ---
        
        // TODO add engine here!
        
        // ---
    }

    public void FinishBuilding() {
        // END of factory,
        //      build all services
        EngineServices.BuildServiceProvider(ServiceCollection);
    }
}