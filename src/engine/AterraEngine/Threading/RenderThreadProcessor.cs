// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Contracts.Nexities.Data.Components;
using AterraCore.Contracts.OmniVault;
using AterraCore.Contracts.Renderer;
using JetBrains.Annotations;
using Raylib_cs;
using Serilog;
using System.Collections.Concurrent;

namespace AterraEngine.Threading;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[UsedImplicitly]
public class RenderThreadProcessor(
    ILogger logger,
    IMainWindow mainWindow,
    IApplicationStageManager applicationStageManager,
    RenderThreadEvents renderThreadEvents,
    ITextureAtlas textureAtlas
) : AbstractThread {
    public ConcurrentQueue<TextureQueueRecord> TextureQueue { get; internal set; } = null!;
    
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public override void Run() {
        mainWindow.Init();

        // Signal back that the opengl context has been created 
        renderThreadEvents.InvokeOpenGlContextCreated();

        while (!Raylib.WindowShouldClose()) {
            // Check for new textures
            if (TextureQueue.TryDequeue(out TextureQueueRecord? textureRecord)) {
                textureAtlas.TryLoadAndRegisterTexture(
                    textureRecord.TextureAssetId, 
                    textureRecord.TexturePath,
                    out ISprite2D? _,
                    predefinedGuid:textureRecord.PredefinedGuid
                );
            }
            
            if (CancellationToken.IsCancellationRequested) break;
            applicationStageManager.GetCurrentFrameProcessor().DrawFrame();
        }

        // Cancellation requested;
        logger.Information("Render Thread Closing");
        Raylib.CloseWindow();

    }
}
