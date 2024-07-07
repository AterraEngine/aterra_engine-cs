// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Common.Types.Nexities;
using AterraCore.DI;
using AterraCore.Loggers;
using static AterraCore.Common.Data.PredefinedAssetIds.NewBootOperationNames;
using static AterraCore.Common.Data.PredefinedAssetIds.NewConfigurationWarnings;

namespace AterraCore.Boot.Operations;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------

public class BuildDependencies : IBootOperation {
    public AssetId AssetId => BuildDependenciesOperation;
    private ILogger Logger { get; } = StartupLogger.CreateLogger(false).ForBootOperationContext("Build Dependencies"); 

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public void Run(BootOperationComponents components) {
        Logger.Debug("Entered Build Service Provider");
        
        var builder = new EngineServiceBuilder(Logger);
        builder.AssignFromServiceDescriptors(components.DefaultServices);
        builder.AssignFromServiceDescriptors(components.DynamicServices);
        builder.AssignFromServiceDescriptors(components.StaticServices);
        
        builder.FinishBuilding();
    }
}
