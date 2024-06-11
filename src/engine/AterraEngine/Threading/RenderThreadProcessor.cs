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
public class RenderThreadProcessor(ILogger logger, IMainWindow mainWindow, IApplicationStageManager applicationStageManager, RenderThreadEvents renderThreadEvents) : AbstractThread {

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public override void Run() {
        mainWindow.Init();

        // Signal back that the opengl context has been created 
        renderThreadEvents.InvokeOpenGlContextCreated();

        while (!Raylib.WindowShouldClose()) {
            if (CancellationToken.IsCancellationRequested) break;
            applicationStageManager.GetCurrentFrameProcessor().DrawFrame();
        }

        // Cancellation requested;
        logger.Information("Render Thread Closing");
        Raylib.CloseWindow();

    }
}
