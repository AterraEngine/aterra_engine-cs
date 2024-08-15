// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Contracts.Nexities.Levels;
using AterraCore.Contracts.Nexities.Systems;
using AterraCore.Contracts.OmniVault.DataCollector;
using AterraCore.Contracts.OmniVault.World;
using AterraCore.DI;
using AterraLib.Nexities.Systems.Rendering;
using CodeOfChaos.Extensions.Serilog;
using JetBrains.Annotations;
using Serilog;
using Serilog.Core;

namespace AterraEngine.Renderer.RaylibCs.FrameProcessors;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[UsedImplicitly]
public class GeneralRenderRaylibFrameProcessor(IAterraCoreWorld world, IDataCollector dataCollector, ILogger logger) : AbstractRaylibFrameProcessor {
    protected override Color ClearColor { get; set; } = new(0, 0, 0, 0);
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    protected override void DrawUi() {
        DrawRectangle(0,0,250,200,Color.White);
        DrawText($"   FPS : {GetFPS()}", 0, 0, 32, Color.DarkBlue);
        DrawText($"   TPS : {GetTps()}", 0, 50, 32, Color.DarkBlue);
        
        if (!world.TryGetActiveLevel(out IActiveLevel? level)) return;
        DrawText($"DUCKS : {level.RawLevelData.ChildrenIDs.Count}",0, 100, 32, Color.DarkBlue);
        DrawText($"ID : {level.RawLevelData.AssetId}",0, 150, 32, Color.DarkBlue);
    }
    private double GetTps() => dataCollector.Tps;

    protected override void Draw2D() {
        // Go into world
        if (!world.TryGetActiveLevel(out IActiveLevel? level)) return;
        
        // Get all logic systems required by active level
        foreach (INexitiesSystem system in level.RenderSystems) {
            system.Tick(world);
            logger.Debug("system {s}", system.AssetId);
        }
    }
}
