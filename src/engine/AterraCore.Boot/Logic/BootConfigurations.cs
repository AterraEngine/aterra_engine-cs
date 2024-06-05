// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Boot.FlexiPlug;
using AterraCore.Boot.Nexities;
using AterraCore.Contracts.Boot;
using Extensions;
using Serilog;
using static AterraCore.Common.Data.ConfigurationWarnings;

namespace AterraCore.Boot.Logic;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------

public static class BootConfigurations {
    public static IEngineConfiguration AddSubConfigurations(this IEngineConfiguration configuration) {
        ILogger logger = configuration.StartupLog;

        configuration.SubConfigurations = new SubConfigurations(
            new FlexiPlugConfiguration(logger, configuration.EngineConfig),
            new NexitiesConfiguration(logger, configuration.EngineConfig)
        );
        
        return configuration;
    }
    
    public static IEngineConfiguration WithSubConfigurations(this IEngineConfiguration configuration, Action<ISubConfigurations> subConfigurationsCallback) {
        ILogger logger = configuration.StartupLog;
        ISubConfigurations subConfigurations = configuration.SubConfigurations;
        
        subConfigurationsCallback.Invoke(subConfigurations);

        if (subConfigurations.FlexiPlug.Warnings != Nominal && configuration.EngineConfig.BootConfig.Exceptions.BreakOnFlowException) {
            logger.ThrowFatal<InvalidOperationException>("Engine configuration for FlexiPlug has warnings: {Warnings}", subConfigurations.FlexiPlug.Warnings);
        } 
        configuration.ServicesDefault.AddLastRepeated(subConfigurations.FlexiPlug.ServicesDefault);
        configuration.ServicesStatic.AddLastRepeated(subConfigurations.FlexiPlug.ServicesStatic);
        
        if (subConfigurations.Nexities.Warnings != Nominal && configuration.EngineConfig.BootConfig.Exceptions.BreakOnFlowException) {
            logger.ThrowFatal<InvalidOperationException>("Engine configuration for Nexities has warnings: {Warnings}", subConfigurations.Nexities.Warnings);
        } 
        configuration.ServicesDefault.AddLastRepeated(subConfigurations.Nexities.ServicesDefault);
        configuration.ServicesStatic.AddLastRepeated(subConfigurations.Nexities.ServicesStatic);
        
        return configuration;
    }
}
