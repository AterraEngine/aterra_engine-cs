// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Common.Types.Nexities;
using AterraCore.Contracts;
using AterraCore.Loggers;
using AterraEngine;
using CodeOfChaos.Extensions;
using Microsoft.Extensions.DependencyInjection;
using static AterraCore.Common.Data.PredefinedAssetIds.NewBootOperationNames;
using static CodeOfChaos.Extensions.DependencyInjection.ServiceDescriptorExtension;

namespace AterraCore.Boot.Operations;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class CollectDependencies : IBootOperation {
    public AssetId AssetId => CollectDependenciesOperation;
    private ILogger Logger { get; } = StartupLogger.CreateLogger(false).ForBootOperationContext("Collect Dependencies");
    
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public void Run(BootOperationComponents components) {
        Logger.Debug("Entered Collection of Dependencies");

        ServiceDescriptor[] defaultDependencies = [
            #region IEngine
            NewServiceDescriptor<IEngine, Engine>(ServiceLifetime.Singleton),
            #endregion
            #region IEngineLogger
            NewServiceDescriptor<ILogger>(EngineLogger.CreateLogger(components.EngineConfigXml.BootConfig.Logging.UseAsyncConsole))
            #endregion
        ];
        
        components.DefaultServices.AddLastRepeated(defaultDependencies);

    }
    
}
