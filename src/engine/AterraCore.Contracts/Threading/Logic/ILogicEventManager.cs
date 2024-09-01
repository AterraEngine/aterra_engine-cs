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
    public event EventHandler<IChangeActiveLevelEventArgs>? EventChangeActiveLevel;
    public event EventHandler<int>? EventTps;
    public event EventHandler<double>? EventDeltaTps;
    public event EventHandler<int>? EventFps;

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public void InvokeChangeActiveLevel(AssetId assetId);
    public void InvokeChangeActiveLevel(IChangeActiveLevelEventArgs eventArgs);
    public void InvokeUpdateTps(int actualTps);
    public void InvokeUpdateDeltaTps(double deltaTps);
    public void InvokeUpdateFps(int fps);
}
