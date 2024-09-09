// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Contracts.Boot.Operations;
using AterraCore.Contracts.FlexiPlug;
using AterraCore.Contracts.OmniVault.Assets;
using AterraCore.Contracts.OmniVault.DataCollector;
using AterraCore.Loggers;
using CodeOfChaos.Extensions;
using Microsoft.Extensions.DependencyInjection;

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

        components.ServiceDescriptors.AddLastRepeated([
            ServiceDescriptor.Singleton(EngineLogger.CreateLogger(components.EngineConfigXml.LoggingConfig.UseAsyncConsole)),
            ServiceDescriptor.Singleton<IPluginAtlas>(provider => provider.GetRequiredService<IPluginAtlasFactory>().GetAtlas()),
            ServiceDescriptor.Singleton<IAssetAtlas>(provider => provider.GetRequiredService<IAssetAtlasFactory>().GetAtlas()),
            ServiceDescriptor.Singleton<IDataCollector>(provider => provider.GetRequiredService<IDataCollectorFactory>().Create()),
            ServiceDescriptor.Singleton(components.EngineConfigXml),
            ServiceDescriptor.Singleton(components)
        ]);

    }
}
