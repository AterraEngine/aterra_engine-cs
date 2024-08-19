// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Contracts.OmniVault.World;
using AterraCore.Contracts.Renderer;
using AterraCore.Contracts.Threading.Logic;
using JetBrains.Annotations;

namespace AterraEngine.Renderer.RaylibCs.FrameProcessors;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[UsedImplicitly]
public abstract class AbstractRaylibFrameProcessor(IAterraCoreWorld world, ILogicEventManager eventManager) : IFrameProcessor {
    protected virtual Color ClearColor => Color.Black;

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public virtual void DrawFrame() {
        BeginDrawing();
        ClearBackground(ClearColor);
        EndDrawing();
        eventManager.InvokeUpdateFps(GetFPS());
    }

    protected virtual void DrawUi(IActiveLevel level) {}
    protected virtual void Draw2D(IActiveLevel level) {}
    protected virtual void Draw3D(IActiveLevel level) {}
}
