// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Contracts.OmniVault.World;
using AterraCore.Contracts.Renderer;
using AterraCore.Contracts.Threading.Logic;
using AterraCore.DI;
using JetBrains.Annotations;

namespace AterraEngine.Renderer.RaylibCs.FrameProcessors;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[UsedImplicitly]
public abstract class AbstractRaylibFrameProcessor : IFrameProcessor {
    protected virtual Color ClearColor => Color.Black;

    private readonly ILogicEventManager _logicEventManager = EngineServices.GetService<ILogicEventManager>();
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public virtual void DrawFrame() {
        BeginDrawing();
        ClearBackground(ClearColor);
        EndDrawing();
        _logicEventManager.InvokeUpdateFps(GetFPS());
    }

    protected virtual void DrawUi(IActiveLevel level) {}
}
