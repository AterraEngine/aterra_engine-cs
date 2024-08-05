// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Contracts;
using AterraCore.Contracts.Boot.Operations;
using AterraCore.Contracts.FlexiPlug;
using AterraCore.Contracts.Nexities.Data.Worlds;
using AterraCore.Contracts.OmniVault;
using AterraCore.Contracts.OmniVault.Assets;
using AterraCore.Contracts.OmniVault.Textures;
using AterraCore.Contracts.Renderer;
using AterraCore.FlexiPlug;
using AterraCore.Loggers;
using AterraCore.Nexities.Worlds;
using AterraCore.OmniVault.Assets;
using AterraCore.OmniVault.Textures;
using AterraEngine;
using AterraEngine.Renderer.RaylibCs;
using AterraEngine.Threading;
using CodeOfChaos.Extensions;
using Microsoft.Extensions.DependencyInjection;
using static CodeOfChaos.Extensions.DependencyInjection.ServiceDescriptorExtension;

namespace AterraCore.Boot.Operations;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class CollectDefaultDependencies : IBootOperation {
    private ILogger Logger { get; } = StartupLogger.CreateLogger(false).ForBootOperationContext(nameof(CollectDefaultDependencies));
    
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public void Run(IBootComponents components) {
        Logger.Debug("Entered Collection of Dependencies");

        ServiceDescriptor[] defaultDependencies = [
            #region Base AterraEngine
            NewServiceDescriptor<IEngine, Engine>(ServiceLifetime.Singleton),
            NewServiceDescriptor<ILogger>(EngineLogger.CreateLogger(components.EngineConfigXml.BootConfig.Logging.UseAsyncConsole)),
            NewServiceDescriptor<RenderThreadEvents, RenderThreadEvents>(ServiceLifetime.Singleton),
            NewServiceDescriptor<IApplicationStageManager, ApplicationStageManager>(ServiceLifetime.Singleton),
            NewServiceDescriptor<IMainWindow, MainWindow>(ServiceLifetime.Singleton),
            #endregion
            #region PluginLoading
            NewServiceDescriptor<IPluginAtlas, PluginAtlas>(ServiceLifetime.Singleton),
            #endregion
            #region Nexities
            NewServiceDescriptor<INexitiesWorld, NexitiesWorld>(ServiceLifetime.Singleton),
            #endregion
            #region OmniVault
            NewServiceDescriptor<IAssetAtlas, AssetAtlas>(ServiceLifetime.Singleton),
            NewServiceDescriptor<IAssetInstanceAtlas, AssetInstanceAtlas>(ServiceLifetime.Singleton),
            
            NewServiceDescriptor<ITextureAtlas, TextureAtlas>(ServiceLifetime.Singleton),
            #endregion
        ];
        
        components.DefaultServices.AddLastRepeated(defaultDependencies);

    }
    
}
