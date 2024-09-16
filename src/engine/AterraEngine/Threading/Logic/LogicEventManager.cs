// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Common.Attributes.DI;
using AterraCore.Contracts.Threading.Logic;
using JetBrains.Annotations;

namespace AterraEngine.Threading.Logic;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[UsedImplicitly]
[Singleton<ILogicEventManager>]
public class LogicEventManager : ILogicEventManager {
    public event EventHandler<int>? EventTps;
    public event EventHandler<double>? EventDeltaTps;
    public event EventHandler<int>? EventFps;

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public void InvokeUpdateTps(int actualTps) => EventTps?.Invoke(this, actualTps);
    public void InvokeUpdateDeltaTps(double deltaTps) => EventDeltaTps?.Invoke(this, deltaTps);
    public void InvokeUpdateFps(int fps) => EventFps?.Invoke(this, fps);
}
