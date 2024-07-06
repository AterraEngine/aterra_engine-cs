// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Contracts.Renderer;
using JetBrains.Annotations;

namespace AterraEngine.Renderer.RaylibCs.FrameProcessors;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[UsedImplicitly]
public abstract class AbstractRaylibFrameProcessor : IFrameProcessor {
    protected virtual Color ClearColor { get; set; } = Color.Black;

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public virtual void DrawFrame() {
        BeginDrawing();
        ClearBackground(ClearColor);
        Draw2D();
        DrawUi();
        EndDrawing();
    }

    protected virtual void DrawUi() {}
    protected virtual void Draw2D() {}
    protected virtual void Draw3D() {}
}
