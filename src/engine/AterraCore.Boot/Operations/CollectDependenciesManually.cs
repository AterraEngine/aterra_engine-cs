// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Contracts.Boot.Operations;
using AterraCore.Contracts.FlexiPlug;
using AterraCore.Contracts.OmniVault.Assets;
using AterraCore.Contracts.OmniVault.DataCollector;
using AterraCore.Contracts.OmniVault.Textures;
using AterraCore.Contracts.OmniVault.World;
using AterraCore.Contracts.OmniVault.World.EntityTree;
using AterraCore.Loggers;
using AterraCore.OmniVault.DataCollector;
using AterraCore.OmniVault.Textures;
using AterraCore.OmniVault.World;
using AterraCore.OmniVault.World.EntityTree;
using AterraCore.OmniVault.World.EntityTree.Pools;
using CodeOfChaos.Extensions;
using Microsoft.Extensions.DependencyInjection;
using static CodeOfChaos.Extensions.DependencyInjection.ServiceDescriptorExtension;

namespace AterraCore.Boot.Operations;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class CollectDependenciesManually : IBootOperation {
    private ILogger Logger { get; } = StartupLogger.CreateLogger(false).ForBootOperationContext(nameof(CollectDependenciesManually));

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public void Run(IBootComponents components) {
        Logger.Debug("Entered Collection of Dependencies");

        List<ServiceDescriptor> dependencies = [
            #region Base AterraEngine
            NewServiceDescriptor<ILogger>(EngineLogger.CreateLogger(components.EngineConfigXml.LoggingConfig.UseAsyncConsole)),

            NewServiceDescriptor<IActiveLevelFactory, ActiveLevelFactory>(ServiceLifetime.Singleton),

            #endregion
            #region PluginLoading
            ServiceDescriptor.Singleton<IPluginAtlas>(provider => provider.GetRequiredService<IPluginAtlasFactory>().GetAtlas()),
            #endregion
            #region OmniVault
            ServiceDescriptor.Singleton<IAssetAtlas>(provider => provider.GetRequiredService<IAssetAtlasFactory>().GetAtlas()),

            NewServiceDescriptor<ITextureAtlas, TextureAtlas>(ServiceLifetime.Singleton),

            NewServiceDescriptor<IAterraCoreWorld, AterraCoreWorld>(ServiceLifetime.Singleton),

            NewServiceDescriptor<IEntityTreeFactory, EntityTreeFactory>(ServiceLifetime.Singleton),
            NewServiceDescriptor<IEntityTreePools, EntityTreePools>(ServiceLifetime.Singleton),
            NewServiceDescriptor<IDataCollectorFactory, DataCollectorFactory>(ServiceLifetime.Singleton),
            ServiceDescriptor.Singleton<IDataCollector>(provider => provider.GetRequiredService<IDataCollectorFactory>().Create()),
            #endregion
            ServiceDescriptor.Singleton(components.EngineConfigXml)
        ];

        components.ServiceDescriptors.AddLastRepeated(dependencies);

    }
}
