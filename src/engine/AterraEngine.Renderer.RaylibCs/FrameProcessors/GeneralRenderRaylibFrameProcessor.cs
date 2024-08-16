// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Contracts.Nexities.Systems;
using AterraCore.Contracts.OmniVault.DataCollector;
using AterraCore.Contracts.OmniVault.World;
using AterraCore.Contracts.Threading.Logic;
using JetBrains.Annotations;
using Serilog;

namespace AterraEngine.Renderer.RaylibCs.FrameProcessors;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[UsedImplicitly]
public class GeneralRenderRaylibFrameProcessor(IAterraCoreWorld world, IDataCollector dataCollector, ILogicEventManager eventManager) : AbstractRaylibFrameProcessor {
    protected override Color ClearColor { get; set; } = new(0, 0, 0, 0);
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    protected override void DrawUi() {
        int fps = GetFPS();

        int i = 0;

        DrawRectangle(0, 0, 250, 50 * 8, Color.White);
        DrawText($"   FPS : {fps}", 0, 50 * i, 32, Color.DarkBlue); i++;
        DrawText($"minFPS : {dataCollector.FpsMin}", 0, 50 * i, 32, Color.DarkBlue);i++;
        DrawText($"maxFPS : {dataCollector.FpsMax}", 0, 50 * i, 32, Color.DarkBlue);i++;
        DrawText($"   TPS : {GetTps()}", 0, 50 * i, 32, Color.DarkBlue);i++;
        
        if (!world.TryGetActiveLevel(out IActiveLevel? level)) return;
        DrawText($"DUCKS : {level.RawLevelData.ChildrenIDs.Count}",0, 50 * i, 32, Color.DarkBlue);i++;
        DrawText($"ID : {level.RawLevelData.AssetId}",0, 50 * i, 12, Color.DarkBlue);i++;
        
        eventManager.InvokeUpdateFps(fps);
    }
    private double GetTps() => dataCollector.Tps;

    protected override void Draw2D() {
        // Go into world
        if (!world.TryGetActiveLevel(out IActiveLevel? level)) return;
        
        // Get all logic systems required by active level
        foreach (INexitiesSystem system in level.RenderSystems) {
            system.Tick(world);
        }
    }
}
