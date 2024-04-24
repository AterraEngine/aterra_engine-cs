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
    public void AssignDefaultServices(IEnumerable<StaticService> services) {
        foreach (StaticService service in services) {
            service(ServiceCollection);
        }
    }
    
    public void AssignStaticServices(IEnumerable<StaticService> services) {
        foreach (StaticService service in services) {
            service(ServiceCollection);
        }
    }

    public void FinishBuilding() {
        // END of factory,
        //      build all services
        EngineServices.BuildServiceProvider(ServiceCollection);
        logger.Information("Dependency Container Built");
    }
}