// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Contracts.OmniVault.Textures;
using AterraCore.Contracts.Renderer;
using AterraCore.Contracts.Threading;
using AterraCore.Contracts.Threading.CTQ;
using AterraCore.Contracts.Threading.CTQ.Dto;
using JetBrains.Annotations;
using Raylib_cs;
using Serilog;

namespace AterraEngine.Threading.Render;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[UsedImplicitly]
public class RenderThreadProcessor(
    ILogger logger,
    IMainWindow mainWindow,
    IApplicationStageManager applicationStageManager,
    RenderThreadEvents renderThreadEvents,
    ITextureAtlas textureAtlas,
    ICrossThreadQueue crossThreadQueue,
    IThreadingManager threadingManager
) : AbstractThread {
    
    private ILogger Logger { get; } = logger.ForContext<RenderThreadProcessor>();

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public override void Run() {
        mainWindow.Init();
        try {
            // Window is actually running now
            while (!Raylib.WindowShouldClose()) {
                HandleQueue();
            
                // Draws the actual frames
                applicationStageManager.GetCurrentFrameProcessor().DrawFrame();
            
                // Wait until the end of the Tick cycle
                // Done by RayLib
            
                // End of Tick
                if (CancellationToken.IsCancellationRequested) break;
            }
        }
        finally {
            Logger.Information("Render Thread Closing");
            Raylib.CloseWindow();
            threadingManager.CancelThreads();
        }
    }
    public override void RegisterEvents() {
        renderThreadEvents.EventApplicationStageChange += applicationStageManager.ReceiveStageChange;
    }

    private void HandleQueue() {
        while (crossThreadQueue.TextureRegistrarQueue.TryDequeue(out TextureRegistrar? textureRecord)) {
            if (textureRecord.UnRegister) textureAtlas.TryUnRegisterTexture(textureRecord.TextureAssetId);
            textureAtlas.TryRegisterTexture(textureRecord.TextureAssetId);
        }
        
        while (crossThreadQueue.TryDequeue(QueueKey.LogicToRender, out Action? action)) {
            action.Invoke();
        }
        while (crossThreadQueue.TryDequeue(QueueKey.MainToRender, out Action? action)) {
            action.Invoke();
        }
    }
}
