// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Contracts.Renderer;
using AterraCore.Contracts.Threading;
using JetBrains.Annotations;

namespace AterraEngine.Threading.Render;
// ---------------------------------------------------------------------------------------------------------------------
// Support Code
// ---------------------------------------------------------------------------------------------------------------------
public class ApplicationStageChangeEventArgs(ApplicationStage applicationStage) : EventArgs, IApplicationStageChangeEventArgs {
    public ApplicationStage ChangeToApplicationStage => applicationStage;
}
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[UsedImplicitly]
public class RenderThreadEvents {
    public event EventHandler<ApplicationStageChangeEventArgs>? EventApplicationStageChange;
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public void InvokeApplicationStageChange(ApplicationStage stage) {
        EventApplicationStageChange?.Invoke(null, new ApplicationStageChangeEventArgs(stage));
    }
}
