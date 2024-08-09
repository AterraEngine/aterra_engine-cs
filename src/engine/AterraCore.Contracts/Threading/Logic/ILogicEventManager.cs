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
    public event EventHandler<double>? EventActualTps;
    public event EventHandler<double>? EventDeltaTps;

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public void InvokeStop();
    public void InvokeStart();
    public void InvokeChangeActiveLevel(AssetId assetId);
    public void InvokeChangeActiveLevel(IChangeActiveLevelEventArgs eventArgs);
    public void InvokeUpdateActualTps(double actualTps);
    public void InvokeUpdateDeltaTps(double actualTps);
}
