// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Contracts.OmniVault.DataCollector;
using AterraCore.Contracts.Threading.Logic;
using Microsoft.Extensions.DependencyInjection;

namespace AterraCore.OmniVault.DataCollector;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public static class DataCollectorFactory {
    public static IDataCollector Create(IServiceProvider serviceProvider) {
        DataCollector dataCollector = RegisterEventCallbacks(
            serviceProvider.GetRequiredService<ILogicEventManager>(),
            new DataCollector()); 
        
        return dataCollector;
    }

    private static DataCollector RegisterEventCallbacks(ILogicEventManager logicEventManager, DataCollector dataCollector) {
        logicEventManager.EventActualTps += (_, d) => dataCollector.Tps = d;
        logicEventManager.EventDeltaTps += (_, d) => dataCollector.DeltaTps = d;
        logicEventManager.EventFps += (_, d) => {
            dataCollector.Fps = d;
            dataCollector.FpsMin = Math.Min(dataCollector.FpsMin, d);
            dataCollector.FpsMax = Math.Max(dataCollector.FpsMax, d);
        };
        
        return dataCollector;
    }
}
