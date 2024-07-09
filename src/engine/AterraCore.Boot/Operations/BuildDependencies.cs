// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Common.Types.Nexities;
using AterraCore.DI;
using AterraCore.Loggers;
using static AterraCore.Common.Data.PredefinedAssetIds.NewBootOperationNames;

namespace AterraCore.Boot.Operations;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------

public class BuildDependencies : IBootOperation {
    public AssetId AssetId => BuildDependenciesOperation;
    public AssetId? RanAfter => CollectDependenciesOperation;
    private ILogger Logger { get; } = StartupLogger.CreateLogger(false).ForBootOperationContext<BuildDependencies>(); 

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public void Run(IBootOperationComponents components) {
        Logger.Debug("Entered Build Service Provider");
        
        var builder = new EngineServiceBuilder(Logger);
        builder.AssignFromServiceDescriptors(components.DefaultServices);
        builder.AssignFromServiceDescriptors(components.DynamicServices);
        builder.AssignFromServiceDescriptors(components.StaticServices);
        
        builder.FinishBuilding();
    }
}
