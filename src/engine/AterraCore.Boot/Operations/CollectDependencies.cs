// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Common.Types.Nexities;
using AterraCore.Contracts;
using AterraCore.Contracts.FlexiPlug;
using AterraCore.Contracts.Nexities.Data.Assets;
using AterraCore.Contracts.Nexities.Data.Worlds;
using AterraCore.FlexiPlug;
using AterraCore.Loggers;
using AterraCore.Nexities.Assets;
using AterraCore.Nexities.Worlds;
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
    public AssetId? RanAfter => RegisterWarningsOperation;
    private ILogger Logger { get; } = StartupLogger.CreateLogger(false).ForBootOperationContext("Collect Dependencies");
    
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public void Run(IBootOperationComponents components) {
        Logger.Debug("Entered Collection of Dependencies");

        ServiceDescriptor[] defaultDependencies = [
            #region IEngine
            NewServiceDescriptor<IEngine, Engine>(ServiceLifetime.Singleton),
            #endregion
            #region IEngineLogger
            NewServiceDescriptor<ILogger>(EngineLogger.CreateLogger(components.EngineConfigXml.BootConfig.Logging.UseAsyncConsole)),
            #endregion
            #region FlexiPlug
            NewServiceDescriptor<IPluginAtlas, PluginAtlas>(ServiceLifetime.Singleton),
            #endregion
            #region Nexities
            NewServiceDescriptor<IAssetAtlas, AssetAtlas>(ServiceLifetime.Singleton),
            NewServiceDescriptor<IAssetInstanceAtlas, AssetInstanceAtlas>(ServiceLifetime.Singleton),
            NewServiceDescriptor<IWorld, World>(ServiceLifetime.Singleton),
            #endregion
        ];
        
        components.DefaultServices.AddLastRepeated(defaultDependencies);

    }
    
}
