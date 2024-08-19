// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Contracts.OmniVault.Textures;
using AterraCore.Contracts.Renderer;
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
    ICrossThreadEvents crossThreadEvents
) : AbstractThread {
    
    private ILogger Logger { get; } = logger.ForContext<RenderThreadProcessor>();

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public override void Run() {
        mainWindow.Init();
        
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
        
        // Actual window is closed;
        crossThreadEvents.InvokeCloseApplication(this);
        if (!Raylib.WindowShouldClose()) return;
        Logger.Information("Render Thread Closing");
        Raylib.CloseWindow();

    }
    public override void RegisterEvents() {
        renderThreadEvents.EventApplicationStageChange += applicationStageManager.ReceiveStageChange;
        crossThreadEvents.OnCloseApplication += HandleClose;
    }

    private void HandleClose(object? sender, EventArgs __) {
        if (sender == this) return;
        if (!Raylib.WindowShouldClose()) return;
        Logger.Information("Render Thread Closing");
        Raylib.CloseWindow();
    }

    private void HandleQueue() {
        // Check for new textures in a batch if needed
        while (crossThreadQueue.TextureRegistrarQueue.TryDequeue(out TextureRegistrar? textureRecord)) {
            if (textureRecord.UnRegister) textureAtlas.TryUnRegisterTexture(textureRecord.TextureAssetId);
            textureAtlas.TryRegisterTexture(textureRecord.TextureAssetId);
        }
    }
}
