// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Common.Types.Nexities;
using AterraCore.Contracts.Threading.Logic.EventDelegates;

namespace AterraCore.Contracts.Threading.Logic;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface ILogicEventManager {
    public event EventHandler? EventStart;
    public event EventHandler? EventStop;
    public event EventHandler<IChangeActiveLevelEventArgs>? EventChangeActiveLevel;
    public event EventHandler<int>? EventActualTps;
    public event EventHandler<double>? EventDeltaTps;
    public event EventHandler<int>? EventFps;

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public void InvokeStop();
    public void InvokeStart();
    public void InvokeChangeActiveLevel(AssetId assetId);
    public void InvokeChangeActiveLevel(IChangeActiveLevelEventArgs eventArgs);
    public void InvokeUpdateActualTps(int actualTps);
    public void InvokeUpdateDeltaTps(double deltaTps);
    public void InvokeUpdateFps(int fps);
}
