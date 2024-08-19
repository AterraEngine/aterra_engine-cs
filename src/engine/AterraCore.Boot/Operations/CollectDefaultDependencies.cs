// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Contracts;
using AterraCore.Contracts.Boot.Operations;
using AterraCore.Contracts.FlexiPlug;
using AterraCore.Contracts.OmniVault.Assets;
using AterraCore.Contracts.OmniVault.Textures;
using AterraCore.Contracts.OmniVault.World;
using AterraCore.Contracts.OmniVault.World.EntityTree;
using AterraCore.Contracts.Renderer;
using AterraCore.Contracts.Threading;
using AterraCore.Contracts.Threading.CTQ;
using AterraCore.Contracts.Threading.Logic;
using AterraCore.FlexiPlug;
using AterraCore.Loggers;
using AterraCore.OmniVault.Assets;
using AterraCore.OmniVault.DataCollector;
using AterraCore.OmniVault.Textures;
using AterraCore.OmniVault.World;
using AterraCore.OmniVault.World.EntityTree;
using AterraCore.OmniVault.World.EntityTree.Pools;
using AterraEngine;
using AterraEngine.Renderer.RaylibCs;
using AterraEngine.Threading;
using AterraEngine.Threading.CTQ;
using AterraEngine.Threading.Logic;
using AterraEngine.Threading.Render;
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
            NewServiceDescriptor<IThreadingManager, ThreadingManager>(ServiceLifetime.Singleton),
            NewServiceDescriptor<ICrossThreadQueue, CrossThreadQueue>(ServiceLifetime.Singleton),
            #endregion
            #region PluginLoading
            NewServiceDescriptor<IPluginAtlas, PluginAtlas>(ServiceLifetime.Singleton),
            #endregion
            #region OmniVault
            NewServiceDescriptor<IAssetAtlas, AssetAtlas>(ServiceLifetime.Singleton),
            NewServiceDescriptor<IAssetInstanceAtlas, AssetInstanceAtlas>(ServiceLifetime.Singleton),
            NewServiceDescriptor<IAssetInstanceFactory, AssetInstanceFactory>(ServiceLifetime.Singleton),
            
            NewServiceDescriptor<ITextureAtlas, TextureAtlas>(ServiceLifetime.Singleton),
            
            NewServiceDescriptor<IAterraCoreWorld, AterraCoreWorld>(ServiceLifetime.Singleton),
            
            NewServiceDescriptor<IEntityTreeFactory, EntityTreeFactory>(ServiceLifetime.Singleton),
            NewServiceDescriptor<IEntityTreePools, EntityTreePools>(ServiceLifetime.Singleton),
            #endregion
            
            NewServiceDescriptor<ILogicEventManager, LogicEventManager>(ServiceLifetime.Singleton),
            ServiceDescriptor.Singleton(DataCollectorFactory.Create), 
        ];
        
        components.DefaultServices.AddLastRepeated(defaultDependencies);

    }
    
}
