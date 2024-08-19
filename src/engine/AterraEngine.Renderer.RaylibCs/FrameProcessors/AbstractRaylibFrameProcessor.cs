// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Contracts.OmniVault.World;
using AterraCore.Contracts.Renderer;
using JetBrains.Annotations;

namespace AterraEngine.Renderer.RaylibCs.FrameProcessors;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[UsedImplicitly]
public abstract class AbstractRaylibFrameProcessor(IAterraCoreWorld world) : IFrameProcessor {
    protected virtual Color ClearColor => Color.Black;

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public virtual void DrawFrame() {
        if (!world.TryGetActiveLevel(out IActiveLevel? level)) return;
        BeginDrawing();
        ClearBackground(ClearColor);
        Draw2D(level);
        DrawUi(level);
        EndDrawing();
    }

    protected virtual void DrawUi(IActiveLevel level) {}
    protected virtual void Draw2D(IActiveLevel level) {}
    protected virtual void Draw3D(IActiveLevel level) {}
}
