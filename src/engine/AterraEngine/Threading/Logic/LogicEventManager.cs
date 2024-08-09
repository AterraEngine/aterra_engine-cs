﻿// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
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
public class LogicEventManager : ILogicEventManager {
    public event EventHandler? EventStart;
    public event EventHandler? EventStop;
    public event EventHandler<IChangeActiveLevelEventArgs>? EventChangeActiveLevel;
    public event EventHandler<double>? EventActualTPS;

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public void InvokeStop() => EventStop?.Invoke(this, EventArgs.Empty);
    public void InvokeStart() => EventStart?.Invoke(this, EventArgs.Empty);
    
    public void InvokeChangeActiveLevel(AssetId assetId) => EventChangeActiveLevel?.Invoke(this, new ChangeActiveLevelEventArgs(assetId));
    public void InvokeChangeActiveLevel(IChangeActiveLevelEventArgs eventArgs) => EventChangeActiveLevel?.Invoke(this, eventArgs);
    
    public void InvokeSendActualTps(double actualTps) => EventActualTPS?.Invoke(this, actualTps);
}
