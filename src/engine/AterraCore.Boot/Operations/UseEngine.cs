// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Common.Types.Nexities;
using AterraCore.Contracts;
using AterraCore.Loggers;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using static AterraCore.Common.Data.PredefinedAssetIds.NewBootOperationNames;
using static AterraCore.Common.Data.PredefinedAssetIds.NewConfigurationWarnings;
using static CodeOfChaos.Extensions.DependencyInjection.ServiceDescriptorExtension;

namespace AterraCore.Boot.Operations;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[UsedImplicitly]
public class UseEngine<T> : IBootOperation where T : class, IEngine {
    public AssetId AssetId => UseEngineOperation;
    public AssetId? RanAfter => null;
    private ILogger Logger { get; } = StartupLogger.CreateLogger(false).ForBootOperationContext("UseEngine"); 

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public void Run(BootOperationComponents components) {
        Logger.Debug("Entered UseEngine");
        if (components.DynamicServices.FirstOrDefault(descriptor => descriptor.ServiceType == typeof(IEngine)) is {} engineServiceDescriptor) {
            // By Default, this is a warning, even if it is set to not break on this.
            components.WarningAtlas.RaiseWarningEvent(EngineOverwritten, this, engineServiceDescriptor.ImplementationType, typeof(T));
        }
        components.DynamicServices.AddFirst(NewServiceDescriptor<IEngine, T>(ServiceLifetime.Singleton));
    }
}
