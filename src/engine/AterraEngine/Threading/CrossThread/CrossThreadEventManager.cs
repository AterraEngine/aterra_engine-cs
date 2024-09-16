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
    public event LevelChangeStarted? EventLevelChangeStarted;
    public event LevelChangeCompleted? EventLevelChangeCompleted;
    
    public void InvokeLevelChangeStarted(IActiveLevel oldLevel) => EventLevelChangeStarted?.Invoke(oldLevel);
    public void InvokeLevelChangeCompleted(IActiveLevel newLevel) => EventLevelChangeCompleted?.Invoke(newLevel);
}