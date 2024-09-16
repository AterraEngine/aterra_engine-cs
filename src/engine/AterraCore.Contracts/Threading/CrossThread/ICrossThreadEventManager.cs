// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Contracts.OmniVault.World;

namespace AterraCore.Contracts.Threading.CrossThread;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface ICrossThreadEventManager {
    #region LevelChange
    public event LevelChangeStarted? EventLevelChangeStarted;
    public event LevelChangeCompleted? EventLevelChangeCompleted;
    
    public void InvokeLevelChangeStarted(IActiveLevel oldLevel);
    public void InvokeLevelChangeCompleted(IActiveLevel newLevel);
    #endregion
    #region TickData Cleanup
    public event Action? LogicTickCleanup;
    public event Action? RenderTickCleanup;
    public event Action? LevelChangeCleanup;
    
    public void InvokeLogicTickCleanup();
    public void InvokeRenderTickCleanup();
    public void InvokeLevelChangeCleanup();
    #endregion
}

