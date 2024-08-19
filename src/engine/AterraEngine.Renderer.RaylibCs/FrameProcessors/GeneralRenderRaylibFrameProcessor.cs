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
public class GeneralRenderRaylibFrameProcessor(IAterraCoreWorld world, IDataCollector dataCollector, ILogicEventManager eventManager) : AbstractRaylibFrameProcessor(world) {
    protected override Color ClearColor { get; } = new(0, 0, 0, 0);
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    protected override void DrawUi(IActiveLevel level) {
        int fps = GetFPS();

        DrawRectangle(0, 0, 250, 50 * 8, Color.White);
        DrawText($"   FPS : {fps}", 0, 0, 32, Color.DarkBlue);
        DrawText($"minFPS : {dataCollector.FpsMin}", 0, 50, 32, Color.DarkBlue);
        DrawText($"maxFPS : {dataCollector.FpsMax}", 0, 100, 32, Color.DarkBlue);
        DrawText($"   TPS : {GetTps()}", 0, 150, 32, Color.DarkBlue);
        
        DrawText($"DUCKS : {level.RawLevelData.ChildrenIDs.Count}",0, 200, 32, Color.DarkBlue);
        DrawText($"ID : {level.RawLevelData.AssetId}",0, 250, 12, Color.DarkBlue);
        
        eventManager.InvokeUpdateFps(fps);
    }
    private double GetTps() => dataCollector.Tps;

    protected override void Draw2D(IActiveLevel level) {
        // Get all logic systems required by active level
        foreach (INexitiesSystem system in level.RenderSystems) 
            system.Tick(level);
    }
}
