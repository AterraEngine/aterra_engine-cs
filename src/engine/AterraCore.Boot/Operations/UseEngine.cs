// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Common.Types.Nexities;
using AterraCore.Contracts;
using AterraCore.Contracts.Boot;
using AterraCore.Loggers;
using AterraEngine;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using static AterraCore.Common.Data.PredefinedAssetIds.NewBootOperationNames;
using static AterraCore.Common.Data.PredefinedAssetIds.NewConfigurationWarnings;
using static CodeOfChaos.Extensions.DependencyInjection.ServiceDescriptorExtension;

namespace AterraCore.Boot.Operations;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class UseEngine<T> : IBootOperation where T : class, IEngine {
    public AssetId AssetId => UseEngineOperation;
    private ILogger Logger { get; } = StartupLogger.CreateLogger(false).ForContext("Section", "BO : UseEngine"); 

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public void Run(BootOperationComponents components) {
        Logger.Debug("Entered UseEngine");
        if (components.StaticServices.FirstOrDefault(descriptor => descriptor.ServiceType == typeof(IEngine)) is {} engineServiceDescriptor) {
            // By Default, this is a warning, even if it is set to not break on this.
            components.WarningAtlas.RaiseWarningEvent(EngineOverwritten, this, engineServiceDescriptor.ImplementationType, typeof(T));
        }
        components.StaticServices.AddFirst(NewServiceDescriptor<IEngine, T>(ServiceLifetime.Singleton));
    }
}

public class UseEngine : UseEngine<Engine>;
