// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace AterraCore.Contracts.Threading;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IThreadingManager {
    IThreadData? LogicThreadData { get;}
    IThreadData? RenderThreadData { get; } 
    
    Task<bool> TrySpawnRenderThreadAsync();
    Task<bool> TrySpawnLogicThreadAsync();

    void CancelThreads();
    WaitHandle[] GetWaitHandles();
    void JoinThreads();
}
