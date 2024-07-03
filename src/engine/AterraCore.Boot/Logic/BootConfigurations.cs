﻿// ---------------------------------------------------------------------------------------------------------------------
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
