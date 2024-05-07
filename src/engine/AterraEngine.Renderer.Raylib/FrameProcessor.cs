// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

using AterraCore.Contracts.Renderer;
using Extensions;
using JetBrains.Annotations;
using Serilog;

namespace AterraEngine.Renderer.Raylib;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------

[UsedImplicitly]
public class FrameProcessor(ILogger logger, IMainWindow mainWindow) : IFrameProcessor{

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public void PostInit() {
        if (!mainWindow.IsInitialised) {
            logger.ThrowFatal<RayLibException>("Main Window not initialised");
        }
    }

    public void DrawFrame() {
        BeginDrawing();
        ClearBackground(Color.White);

        DrawText("Hello, world!", 12, 12, 20, Color.Black);

        EndDrawing();
    }
}