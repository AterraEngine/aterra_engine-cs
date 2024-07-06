// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Contracts.Threading;

namespace AterraCore.Contracts.Renderer;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IApplicationStageManager {
    IFrameProcessor GetCurrentFrameProcessor();
    void ReceiveStageChange(object? sender, IApplicationStageChangeEventArgs eventArgs);
    IDictionary<ApplicationStage, IFrameProcessor> TryRegisterStage(ApplicationStage stage, IFrameProcessor frameProcessor);
}
