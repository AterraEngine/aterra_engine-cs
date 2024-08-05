// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Contracts.Nexities.Data.Worlds;
using AterraCore.DI;
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
    private Render2D Render2D { get; } = EngineServices.CreateWithServices<Render2D>();
    
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    protected override void DrawUi() {
        DrawFPS(0,0);
        if (world.LoadedLevel is null) return;
        DrawText(world.LoadedLevel.AssetTree.Count.ToString(),50, 50, 32, Color.Lime );
    }

    protected override void Draw2D() {
        if (world.LoadedLevel is null) return;
        Render2D.ProcessLevel(world.LoadedLevel);
    }
}
