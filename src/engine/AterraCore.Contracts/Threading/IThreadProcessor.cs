// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Contracts.OmniVault.World;

namespace AterraCore.Contracts.Threading;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IThreadProcessor {
    CancellationToken CancellationToken { get; set; }
    void RegisterEventsStartup();
    void Run();

    void OnLevelChangeStarted(IActiveLevel oldLevel);
    void OnLevelChangeCompleted(IActiveLevel newLevel);
}
