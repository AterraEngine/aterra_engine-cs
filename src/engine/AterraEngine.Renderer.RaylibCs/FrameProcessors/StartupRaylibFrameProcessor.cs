// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

using System.Numerics;
using AterraCore.Contracts.FlexiPlug;
using JetBrains.Annotations;
using Serilog;
namespace AterraEngine.Renderer.RaylibCs.FrameProcessors;

using AterraCore.Contracts.Nexities.Data.Assets;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------

[UsedImplicitly]
public class StartupRaylibFrameProcessor(IAssetAtlas assetAtlas, IPluginAtlas pluginAtlas) : AbstractRaylibFrameProcessor {
    protected override Color ClearColor { get; set; } = new(0, 0, 0, 0);

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    protected override void DrawUi() {
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
            Color.Red);

        DrawRectanglePro(
            new Rectangle(
                new Vector2(screenWidth / 2 - barMaxWidth / 2, 5 * (screenHeight / 10) - barHeight),
                new Vector2(progress, barHeight)),
            Vector2.Zero,
            0,
            Color.Yellow);

        // DrawRectangleLines(0, 0, (int)maxWidth, 10, Color.Yellow);
        // DrawRectangle(0, 0, (int)progress, 10, Color.White);
    }
}