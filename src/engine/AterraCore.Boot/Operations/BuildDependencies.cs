// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Contracts.Boot.Operations;
using AterraCore.DI;
using AterraCore.Loggers;

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

        builder.AssignFromServiceDescriptors(components.DefaultServices);
        builder.AssignFromServiceDescriptors(components.DynamicServices);
        builder.AssignFromServiceDescriptors(components.StaticServices);

        builder.FinishBuilding();
    }
}
