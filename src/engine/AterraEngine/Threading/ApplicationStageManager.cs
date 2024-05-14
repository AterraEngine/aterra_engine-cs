// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

using AterraCore.Contracts.Renderer;
using AterraCore.Contracts.Threading;
using Extensions;
using JetBrains.Annotations;

namespace AterraEngine.Threading;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------

[UsedImplicitly]
public class ApplicationStageManager : IApplicationStageManager {
    private ApplicationStage _currentApplicationStage = ApplicationStage.Undefined;
    private IFrameProcessor? _cachedFrameProcessor;
    private readonly Dictionary<ApplicationStage, IFrameProcessor> _frameProcessors = new();
    
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public IFrameProcessor GetCurrentFrameProcessor() {
        if(!_frameProcessors.TryGetValue(_currentApplicationStage, out IFrameProcessor? frameProcessor)) {
            frameProcessor = _frameProcessors[ApplicationStage.Undefined];
        }
        return _cachedFrameProcessor ??= frameProcessor;
    }

    public void ReceiveStageChange(object? _, IApplicationStageChangeEventArgs eventArgs) {
        _currentApplicationStage = eventArgs.ChangeToApplicationStage;
        _cachedFrameProcessor = null;
    }

    public bool TryRegisterStage(ApplicationStage stage, IFrameProcessor frameProcessor) {
        return _frameProcessors.TryAddOrUpdate(stage, frameProcessor);
    }
}