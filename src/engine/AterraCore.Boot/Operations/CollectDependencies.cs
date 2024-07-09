// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Common.Types.Nexities;
using AterraCore.Contracts;
using AterraCore.Contracts.FlexiPlug;
using AterraCore.Contracts.Nexities.Data.Assets;
using AterraCore.Contracts.Nexities.Data.Worlds;
using AterraCore.Contracts.OmniVault;
using AterraCore.Contracts.Renderer;
using AterraCore.FlexiPlug;
using AterraCore.Loggers;
using AterraCore.Nexities.Assets;
using AterraCore.Nexities.Worlds;
using AterraCore.OmniVault.Textures;
using AterraEngine;
using AterraEngine.Renderer.RaylibCs;
using AterraEngine.Threading;
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
    private ILogger Logger { get; } = StartupLogger.CreateLogger(false).ForBootOperationContext(nameof(CollectDependencies));
    
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public void Run(IBootOperationComponents components) {
        Logger.Debug("Entered Collection of Dependencies");

        ServiceDescriptor[] defaultDependencies = [
            #region Base AterraEngine
            NewServiceDescriptor<IEngine, Engine>(ServiceLifetime.Singleton),
            NewServiceDescriptor<ILogger>(EngineLogger.CreateLogger(components.EngineConfigXml.BootConfig.Logging.UseAsyncConsole)),
            NewServiceDescriptor<RenderThreadEvents, RenderThreadEvents>(ServiceLifetime.Singleton),
            NewServiceDescriptor<IApplicationStageManager, ApplicationStageManager>(ServiceLifetime.Singleton),
            NewServiceDescriptor<IMainWindow, MainWindow>(ServiceLifetime.Singleton),
            #endregion
            #region FlexiPlug
            NewServiceDescriptor<IPluginAtlas, PluginAtlas>(ServiceLifetime.Singleton),
            #endregion
            #region Nexities
            NewServiceDescriptor<IAssetAtlas, AssetAtlas>(ServiceLifetime.Singleton),
            NewServiceDescriptor<IAssetInstanceAtlas, AssetInstanceAtlas>(ServiceLifetime.Singleton),
            NewServiceDescriptor<IWorld, World>(ServiceLifetime.Singleton),
            #endregion
            #region OmniVault
            NewServiceDescriptor<ITextureAtlas, TextureAtlas>(ServiceLifetime.Singleton),
            #endregion
        ];
        
        components.DefaultServices.AddLastRepeated(defaultDependencies);

    }
    
}
