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
        return _cachedFrameProcessor ??= _frameProcessors[_currentApplicationStage];
    }

    public void ReceiveStageChange(object? _, IApplicationStageChangeEventArgs eventArgs) {
        _currentApplicationStage = eventArgs.ChangeToApplicationStage;
        _cachedFrameProcessor = _frameProcessors[_currentApplicationStage];
    }

    public bool TryRegisterStage(ApplicationStage stage, IFrameProcessor frameProcessor) {
        return _frameProcessors.TryAddOrUpdate(stage, frameProcessor);
    }
}