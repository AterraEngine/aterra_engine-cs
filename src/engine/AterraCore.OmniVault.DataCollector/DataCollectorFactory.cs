﻿// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Contracts.OmniVault.DataCollector;
using AterraCore.Contracts.Threading.Logic;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace AterraCore.OmniVault.DataCollector;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public static class DataCollectorFactory {
    public static IDataCollector Create(IServiceProvider serviceProvider) {
        var dataCollector = new DataCollector(serviceProvider.GetRequiredService<ILogger>());
        
        var logicEventManager = serviceProvider.GetRequiredService<ILogicEventManager>();
        logicEventManager.EventActualTPS += (_, d) => dataCollector.TPS = d;
        
        return dataCollector;
    }
}
