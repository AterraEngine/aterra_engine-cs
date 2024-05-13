// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

using AterraCore.Common;
using AterraCore.Common.Nexities;
using AterraCore.Contracts;
using AterraCore.Contracts.FlexiPlug;
using AterraCore.Contracts.Nexities.Assets;
using AterraCore.Contracts.Nexities.Levels;
using AterraCore.Contracts.Nexities.Worlds;
using AterraCore.Contracts.Renderer;
using AterraCore.DI;
using AterraEngine.Threading;
using Extensions;
using JetBrains.Annotations;
using Serilog;

namespace AterraEngine;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[UsedImplicitly]
public class Engine(
    ILogger logger, IAssetAtlas assetAtlas, IAssetInstanceAtlas instanceAtlas, IPluginAtlas pluginAtlas, IWorld world,
    RenderThreadEvents renderThreadEvents, IApplicationStageManager applicationStageManager
    ) : IEngine {
    private readonly TaskCompletionSource<bool> _openGlContextCreated = new();
    private readonly CancellationTokenSource _ctsRenderThread = new();
    
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    private async Task HandleFatalExceptionGracefully() {
        await _ctsRenderThread.CancelAsync();
        
        logger.ExitFatal(ExitCodes.GeneralError, "Fatally Crashing gracefully");
    }
    
    public bool TryAssignStartingLevel(AssetId assetId) {
        return instanceAtlas.TryCreateInstance(assetId, out ILevel? level)
               && world.TryLoadLevel(level);
    }

    public IEngine SubscribeToEvents() {
        renderThreadEvents.EventApplicationStageChange += applicationStageManager.ReceiveStageChange;
        
        renderThreadEvents.EventOpenGlContextCreated += (_, _) => {_openGlContextCreated.SetResult(true);};
        renderThreadEvents.EventOpenGlContextCreated += (_, _) => {logger.Information("OpenGL Context Created"); };
        return this;
    }

    public IEngine SpawnRenderThread() {
        var renderThreadProcessor = EngineServices.CreateWithServices<RenderThreadProcessor>();
        renderThreadProcessor.CancellationToken = _ctsRenderThread.Token;
        var renderThread = new Thread(renderThreadProcessor.Run);
        renderThread.Start();
        logger.Information("Spawned RenderThread");

        return this;
    }
    
    public async Task Run() {
        logger.Information("Entered AterraEngine");
        
        // Don't wait forever for the opengl context to be created, else we will have many issues
        try {
            var cts = new CancellationTokenSource(5000);
            await _openGlContextCreated.Task.WithCancellation(cts.Token);
        }
        catch  (OperationCanceledException ex) {
            logger.Fatal(ex, "Render Thread did not signal back of successful window creation before the timeout period");
            await HandleFatalExceptionGracefully();
        }
        
        foreach (AssetRegistration assetRegistration in pluginAtlas.GetAssetRegistrations()) {
            
            await Task.Delay(5000);
            
            if (!assetAtlas.TryAssignAsset(assetRegistration, out AssetId? _)) {
                logger.Warning("Type {Type} could not be assigned as an asset", assetRegistration.Type);
            }
        }
        
        // TODO create window etc...

        
        // Task.Run(() => {
        //     Task.Delay(5000, cts.Token);
        //     _ctsRenderThread.Cancel();
        // }, _ctsRenderThread.Token);
    }
}