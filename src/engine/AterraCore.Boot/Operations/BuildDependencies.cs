// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Contracts.Boot.Operations;
using AterraCore.DI;
using AterraCore.Loggers;
using Microsoft.Extensions.DependencyInjection;

namespace AterraCore.Boot.Operations;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class BuildDependencies : IBootOperation {
    private ILogger Logger { get; } = StartupLogger.CreateLogger(false).ForBootOperationContext<BuildDependencies>();

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public void Run(IBootComponents components) {
        Logger.Debug("Entered Build Service Provider");

        var builder = new EngineServiceBuilder(Logger, components.Services);
        
        builder.AssignFromServiceDescriptors(components.ServiceDescriptors);
        components.Services.AddSingleton(components);

        builder.FinishBuilding();
    }
}
