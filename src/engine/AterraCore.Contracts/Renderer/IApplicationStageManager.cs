// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

using AterraCore.Contracts.Threading;

namespace AterraCore.Contracts.Renderer;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------

public interface IApplicationStageManager {
    public IFrameProcessor GetCurrentFrameProcessor();
    public void ReceiveStageChange(object? sender, IApplicationStageChangeEventArgs eventArgs);
    public bool TryRegisterStage(ApplicationStage stage, IFrameProcessor frameProcessor);
}