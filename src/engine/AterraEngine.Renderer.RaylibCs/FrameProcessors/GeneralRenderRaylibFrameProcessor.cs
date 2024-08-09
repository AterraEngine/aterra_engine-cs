﻿// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Contracts.Nexities.Data.Levels;
using AterraCore.Contracts.Nexities.Data.Worlds;
using AterraCore.Contracts.OmniVault.DataCollector;
using AterraCore.DI;
using AterraLib.Nexities.Systems;
using JetBrains.Annotations;
using Serilog;

namespace AterraEngine.Renderer.RaylibCs.FrameProcessors;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[UsedImplicitly]
public class GeneralRenderRaylibFrameProcessor(ILogger logger, INexitiesWorld world, IDataCollector dataCollector) : AbstractRaylibFrameProcessor {
    protected override Color ClearColor { get; set; } = new(0, 0, 0, 0);
    private Render2D Render2D { get; } = EngineServices.CreateWithServices<Render2D>();
    
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    protected override void DrawUi() {
        DrawRectangle(0,0,250,150,Color.White);
        DrawText($"   FPS : {GetFPS()}", 0, 0, 32, Color.DarkBlue);
        DrawText($"   TPS : {GetTps()}", 0, 50, 32, Color.DarkBlue);
        if (!world.TryGetActiveLevel(out INexitiesLevel? level)) return;
        DrawText($"DUCKS : {level.AssetTree.Count}",0, 100, 32, Color.DarkBlue);
    }
    private double GetTps() => dataCollector.TPS;

    protected override void Draw2D() {
        if (!world.TryGetActiveLevel(out INexitiesLevel? level)) return;
        Render2D.ProcessLevel(level);
    }
}
