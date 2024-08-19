// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Contracts.FlexiPlug;
using AterraCore.Contracts.OmniVault.Assets;
using AterraCore.Contracts.OmniVault.World;
using AterraCore.Contracts.Threading.Logic;
using JetBrains.Annotations;
using System.Numerics;

namespace AterraEngine.Renderer.RaylibCs.FrameProcessors;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[UsedImplicitly]
public class StartupRaylibFrameProcessor(IAssetAtlas assetAtlas, IPluginAtlas pluginAtlas, IAterraCoreWorld world, ILogicEventManager eventManager) : AbstractRaylibFrameProcessor(world,eventManager) {
    protected override Color ClearColor { get; } = new(0, 0, 0, 0);

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public override void DrawFrame() {
        if (!world.TryGetActiveLevel(out IActiveLevel? level)) return;
        BeginDrawing();
        ClearBackground(ClearColor);
        DrawUi(level);
        EndDrawing();
        eventManager.InvokeUpdateFps(GetFPS());
    }
    
    protected override void DrawUi(IActiveLevel level) {
        float screenWidth = GetRenderWidth();
        float screenHeight = GetRenderHeight();

        const float barMaxWidth = 100;
        const float barHeight = 10;
        float progress = assetAtlas.TotalCount * (barMaxWidth / pluginAtlas.TotalAssetCount);

        // logger.Information("{a},{b}, {c}", screenWidth, screenHeight, new Vector2(screenWidth / 2, 0)); 
        DrawRectanglePro(
            new Rectangle(
            new Vector2(screenWidth / 2 - barMaxWidth / 2, 5 * (screenHeight / 10) - barHeight),
            new Vector2(barMaxWidth, barHeight)),
            Vector2.Zero,
            0,
            Color.Red
        );

        DrawRectanglePro(
            new Rectangle(
            new Vector2(screenWidth / 2 - barMaxWidth / 2, 5 * (screenHeight / 10) - barHeight),
            new Vector2(progress, barHeight)),
            Vector2.Zero,
            0,
            Color.Yellow
        );

        // DrawRectangleLines(0, 0, (int)maxWidth, 10, Color.Yellow);
        // DrawRectangle(0, 0, (int)progress, 10, Color.White);
    }
}
