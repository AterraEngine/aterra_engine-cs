// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Common.Attributes.DI;
using AterraCore.Contracts.OmniVault.DataCollector;
using AterraCore.Contracts.Threading.Logic;
using JetBrains.Annotations;

namespace AterraEngine.Threading.CrossThread;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[UsedImplicitly]
[Singleton<IDataCollectorFactory>]
public class DataCollectorFactory(ILogicEventManager logicEventManager) : IDataCollectorFactory {
    public IDataCollector Create() {
        var dataCollector = new DataCollector();
        RegisterEventCallbacks(dataCollector);
        return dataCollector;
    }

    private void RegisterEventCallbacks(DataCollector dataCollector) {
        logicEventManager.EventTps += (_, d) => dataCollector.Tps = d;
        logicEventManager.EventTps += (_, d) => dataCollector.TpsMin = Math.Min(dataCollector.FpsMin, d);
        logicEventManager.EventTps += (_, d) => dataCollector.TpsMax = Math.Max(dataCollector.FpsMax, d);
        logicEventManager.EventTps += (_, d) => dataCollector.UpdateTpsAverage(d);

        logicEventManager.EventDeltaTps += (_, d) => dataCollector.DeltaTps = d;

        logicEventManager.EventFps += (_, d) => dataCollector.Fps = d;
        logicEventManager.EventFps += (_, d) => dataCollector.FpsMin = Math.Min(dataCollector.FpsMin, d);
        logicEventManager.EventFps += (_, d) => dataCollector.FpsMax = Math.Max(dataCollector.FpsMax, d);
        logicEventManager.EventFps += (_, d) => dataCollector.UpdateFpsAverage(d);
    }
}
