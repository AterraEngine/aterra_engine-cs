// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Contracts.Nexities.Systems;
using AterraCore.Contracts.OmniVault.Textures;
using AterraCore.Contracts.OmniVault.World;
using AterraCore.Contracts.Renderer;
using JetBrains.Annotations;
using Raylib_cs;
using Serilog;
using System.Collections.Concurrent;

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
    IAterraCoreWorld world
) : AbstractThread {
    
    private ILogger Logger { get; } = logger.ForContext<RenderThreadProcessor>();
    public ConcurrentQueue<TextureQueueRecord> TextureQueue { get; internal set; } = null!;

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public override void Run() {
        mainWindow.Init();
        
        // Wait until the main thread signaled to start
        // Signal back that the opengl context has been created 
        renderThreadEvents.InvokeOpenGlContextCreated();
        
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
        
        // Cancellation requested;
        Logger.Information("Render Thread Closing");
        Raylib.CloseWindow();

    }

    private void HandleQueue() {
        if (TextureQueue.IsEmpty) return;
        
        // Check for new textures in a batch if needed
        while (TextureQueue.TryDequeue(out TextureQueueRecord? textureRecord)) {
            textureAtlas.TryRegisterTexture(textureRecord.TextureAssetId);
        }
    }
}
