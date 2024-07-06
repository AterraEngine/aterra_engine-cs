// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Contracts.Boot;
using CodeOfChaos.Extensions;
using CodeOfChaos.Extensions.Serilog;
using Serilog;
using static AterraCore.Common.Data.ConfigurationWarnings;

namespace AterraCore.Boot.Logic;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public static class BootConfigurations {
    public static IEngineConfiguration WithSubConfigurations(
        this IEngineConfiguration configuration,
        Action<ISubConfigurations> subConfigurationsCallback
    ) {
        ILogger logger = configuration.StartupLog;
        ISubConfigurations subConfigurations = configuration.SubConfigurations;
        
        subConfigurationsCallback.Invoke(subConfigurations);

        foreach (IBootConfiguration bootConfiguration in subConfigurations) {
            if (bootConfiguration.Warnings != Nominal && configuration.EngineConfig.BootConfig.Exceptions.BreakOnFlowException) {
                logger.ThrowFatal<InvalidOperationException>(
                    "Engine configuration for {Type} has warnings: {Warnings}",
                    bootConfiguration.GetType().FullName, 
                    bootConfiguration.Warnings
                );
            } 
            configuration.ServicesDefault.AddLastRepeated(bootConfiguration.ServicesDefault);
            configuration.ServicesStatic.AddLastRepeated(bootConfiguration.ServicesStatic);
        }
        
        return configuration;
    }
}
