// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

using AterraCore.Contracts.Renderer;
using AterraCore.Contracts.Threading;
using CodeOfChaos.Extensions;
using JetBrains.Annotations;

namespace AterraEngine.Threading;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------

[UsedImplicitly]
public class ApplicationStageManager : IApplicationStageManager {
    private readonly Dictionary<ApplicationStage, IFrameProcessor> _frameProcessors = new();
    private IFrameProcessor? _cachedFrameProcessor;
    private ApplicationStage _currentApplicationStage = ApplicationStage.Undefined;

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public IFrameProcessor GetCurrentFrameProcessor() {
        if (!_frameProcessors.TryGetValue(_currentApplicationStage, out IFrameProcessor? frameProcessor)) {
            frameProcessor = _frameProcessors[ApplicationStage.Undefined];
        }
        return _cachedFrameProcessor ??= frameProcessor;
    }

    public void ReceiveStageChange(object? _, IApplicationStageChangeEventArgs eventArgs) {
        _currentApplicationStage = eventArgs.ChangeToApplicationStage;
        _cachedFrameProcessor = null;
    }

    public IDictionary<ApplicationStage, IFrameProcessor> TryRegisterStage(ApplicationStage stage, IFrameProcessor frameProcessor) => _frameProcessors.AddOrUpdate(stage, frameProcessor);
}
