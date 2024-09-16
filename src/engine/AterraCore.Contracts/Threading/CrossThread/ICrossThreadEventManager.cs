// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Contracts.OmniVault.World;

namespace AterraCore.Contracts.Threading.CrossThread;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface ICrossThreadEventManager {
    event LevelChangeStarted? EventLevelChangeStarted;
    event LevelChangeCompleted? EventLevelChangeCompleted;

    void InvokeLevelChangeStarted(IActiveLevel oldLevel);
    void InvokeLevelChangeCompleted(IActiveLevel newLevel);
}

