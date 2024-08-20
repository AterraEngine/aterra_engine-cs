// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Contracts.Nexities.Systems;
using AterraCore.Contracts.OmniVault.DataCollector;
using AterraCore.Contracts.OmniVault.World;
using AterraCore.Contracts.Threading.Logic;
using JetBrains.Annotations;

namespace AterraEngine.Renderer.RaylibCs.FrameProcessors;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[UsedImplicitly]
public class GeneralRenderRaylibFrameProcessor(IAterraCoreWorld world, IDataCollector dataCollector, ILogicEventManager eventManager) 
    : AbstractRaylibFrameProcessor {
    
    protected override Color ClearColor { get; } = new(0, 0, 0, 0);
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    protected override void DrawUi(IActiveLevel level) {
        DrawRectangle(0, 0, 250, 50 * 9, Color.White);
        DrawText($"   FPS : {dataCollector.Fps}", 0, 0, 32, Color.DarkBlue);
        DrawText($"minFPS : {dataCollector.FpsMin}", 0, 50, 32, Color.DarkBlue);
        DrawText($"maxFPS : {dataCollector.FpsMax}", 0, 100, 32, Color.DarkBlue);
        DrawText($"avgFPS : {dataCollector.FpsAverage:N2}", 0, 150, 32, Color.DarkBlue);
        DrawText($"   TPS : {dataCollector.Tps}", 0, 200, 32, Color.DarkBlue);
        DrawText($"avgTPS : {dataCollector.TpsAverage:N2}", 0, 250, 32, Color.DarkBlue);
        
        DrawText($"DUCKS : {level.RawLevelData.ChildrenIDs.Count}",0, 300, 32, Color.DarkBlue);
        DrawText($"ID : {level.RawLevelData.AssetId}",0, 350, 12, Color.DarkBlue);
    }
    
    public override void DrawFrame() {
        if (!world.TryGetActiveLevel(out IActiveLevel? level)) return;
        BeginDrawing();
        ClearBackground(ClearColor);
        
        BeginMode2D(level.Camera2DEntity.Camera);
        foreach (INexitiesSystem system in level.RenderSystems) 
            system.Tick(level);
        EndMode2D();
        
        DrawUi(level);
        EndDrawing();
        eventManager.InvokeUpdateFps(GetFPS());
    }
}
