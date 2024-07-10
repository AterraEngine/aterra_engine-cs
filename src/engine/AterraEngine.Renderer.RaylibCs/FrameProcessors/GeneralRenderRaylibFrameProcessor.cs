// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Contracts.Nexities.Data.Worlds;
using AterraLib.Nexities.Systems;
using JetBrains.Annotations;
using Serilog;

namespace AterraEngine.Renderer.RaylibCs.FrameProcessors;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[UsedImplicitly]
public class GeneralRenderRaylibFrameProcessor(ILogger logger, INexitiesWorld world) : AbstractRaylibFrameProcessor {
    protected override Color ClearColor { get; set; } = new(0, 0, 0, 0);
    private Render2D Render2D { get; } = new(logger);
    
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    protected override void DrawUi() {
        DrawFPS(0,0);
    }

    protected override void Draw2D() {
        if (world.LoadedLevel is null) return;
        Render2D.ProcessLevel(world.LoadedLevel);
        DrawText(world.LoadedLevel.ChildEntities.Count.ToString(),50, 50, 32, Color.Red );
        DrawText(Render2D.DrawCalls.ToString(),50, 100, 32, Color.Red );
        
    }
}
