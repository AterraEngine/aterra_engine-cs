// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

using AterraCore.Contracts.Renderer;
using AterraCore.Contracts.Threading;
using JetBrains.Annotations;

namespace AterraEngine.Threading;

// ---------------------------------------------------------------------------------------------------------------------
// Support Code
// ---------------------------------------------------------------------------------------------------------------------
public class ApplicationStageChangeEventArgs(ApplicationStage applicationStage) : EventArgs, IApplicationStageChangeEventArgs{
    public ApplicationStage ChangeToApplicationStage => applicationStage;
}

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[UsedImplicitly]
public class RenderThreadEvents {
    public event EventHandler<EventArgs>? EventOpenGlContextCreated;
    public event EventHandler<ApplicationStageChangeEventArgs>? EventApplicationStageChange;

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public void InvokeOpenGlContextCreated() {
        EventOpenGlContextCreated?.Invoke(null, EventArgs.Empty);
    }

    public void InvokeApplicationStageChange(ApplicationStage stage) {
        EventApplicationStageChange?.Invoke(null, new ApplicationStageChangeEventArgs(stage));
    }
}