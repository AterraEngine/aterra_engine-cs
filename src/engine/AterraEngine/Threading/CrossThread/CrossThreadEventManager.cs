// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Common.Attributes.DI;
using AterraCore.Contracts.OmniVault.World;
using AterraCore.Contracts.Threading;
using AterraCore.Contracts.Threading.CrossThread;
using JetBrains.Annotations;

namespace AterraEngine.Threading.CrossThread;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[UsedImplicitly]
[Singleton<CrossThreadEventManager, ICrossThreadEventManager>]
public class CrossThreadEventManager : ICrossThreadEventManager {
    #region LevelChange
    public event LevelChangeStarted? EventLevelChangeStarted;
    public event LevelChangeCompleted? EventLevelChangeCompleted;

    public void InvokeLevelChangeStarted(IActiveLevel oldLevel) => EventLevelChangeStarted?.Invoke(oldLevel);
    public void InvokeLevelChangeCompleted(IActiveLevel newLevel) => EventLevelChangeCompleted?.Invoke(newLevel);
    #endregion
    #region TickData Cleanup
    public event Action? LogicTickCleanup;
    public event Action? RenderTickCleanup;
    public event Action? LevelChangeCleanup;

    public void InvokeLogicTickCleanup() => LogicTickCleanup?.Invoke();
    public void InvokeRenderTickCleanup() => RenderTickCleanup?.Invoke();
    public void InvokeLevelChangeCleanup() => LevelChangeCleanup?.Invoke();
    #endregion
}
