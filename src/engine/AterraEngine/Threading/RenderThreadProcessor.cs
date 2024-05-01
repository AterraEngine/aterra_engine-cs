// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Contracts.Renderer;
using JetBrains.Annotations;
using Raylib_cs;
using Serilog;

namespace AterraEngine.Threading;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[UsedImplicitly]
public class RenderThreadProcessor(ILogger logger, IMainWindow mainWindow, IFrameProcessor frameProcessor) : AbstractThread {
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public override void Run() {
        mainWindow.Init();
        frameProcessor.PostInit();

        while (!Raylib.WindowShouldClose()) {
            if (CancellationToken.IsCancellationRequested) break;
            frameProcessor.DrawFrame();
        }
        
        // Cancellation requested;
        logger.Information("Render Thread Closing");
        Raylib.CloseWindow();
    }
}