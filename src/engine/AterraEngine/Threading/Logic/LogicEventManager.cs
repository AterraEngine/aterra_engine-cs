// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Attributes;
using AterraCore.Common.Types.Nexities;
using AterraCore.Contracts.Threading.Logic;
using AterraCore.Contracts.Threading.Logic.EventDelegates;
using AterraEngine.Threading.Logic.EventDelegates;
using JetBrains.Annotations;

namespace AterraEngine.Threading.Logic;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[UsedImplicitly]
[Singleton<ILogicEventManager>]
public class LogicEventManager : ILogicEventManager {
    public event EventHandler<IChangeActiveLevelEventArgs>? EventChangeActiveLevel;
    public event EventHandler<int>? EventTps;
    public event EventHandler<double>? EventDeltaTps;
    public event EventHandler<int>? EventFps;

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public void InvokeChangeActiveLevel(AssetId assetId) => EventChangeActiveLevel?.Invoke(this, new ChangeActiveLevelEventArgs(assetId));
    public void InvokeChangeActiveLevel(IChangeActiveLevelEventArgs eventArgs) => EventChangeActiveLevel?.Invoke(this, eventArgs);

    public void InvokeUpdateTps(int actualTps) => EventTps?.Invoke(this, actualTps);
    public void InvokeUpdateDeltaTps(double deltaTps) => EventDeltaTps?.Invoke(this, deltaTps);
    public void InvokeUpdateFps(int fps) => EventFps?.Invoke(this, fps);
}
