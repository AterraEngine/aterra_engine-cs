// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Common.Data;
using AterraCore.Common.Types.Nexities;
using AterraCore.Contracts;
using AterraCore.Contracts.FlexiPlug;
using AterraCore.Contracts.Nexities.Data.Assets;
using AterraCore.Contracts.Nexities.Data.Components;
using AterraCore.Contracts.Nexities.Data.Levels;
using AterraCore.Contracts.Nexities.Data.Worlds;
using AterraCore.Contracts.Renderer;
using AterraCore.DI;
using AterraEngine.Threading;
using AterraLib.Nexities.Entities;
using CodeOfChaos.Extensions;
using CodeOfChaos.Extensions.Serilog;
using JetBrains.Annotations;
using Serilog;
using System.Collections.Concurrent;
using System.Numerics;

namespace AterraEngine;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[UsedImplicitly]
public class Engine(
    ILogger logger,
    IAssetAtlas assetAtlas,
    IAssetInstanceAtlas instanceAtlas,
    IPluginAtlas pluginAtlas,
    INexitiesWorld world,
    RenderThreadEvents renderThreadEvents,
    IApplicationStageManager applicationStageManager
) : IEngine {
    private readonly CancellationTokenSource _ctsRenderThread = new();
    private readonly TaskCompletionSource<bool> _openGlContextCreated = new();
    private readonly ConcurrentQueue<TextureQueueRecord> _textureQueue = new();
    private ILogger Logger { get; } = logger.ForContext<Engine>();
    
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public bool TryAssignStartingLevel(AssetId assetId) =>
        instanceAtlas.TryCreate(assetId, out INexitiesLevel? level)
        && world.TryLoadLevel(level);

    public IEngine SubscribeToEvents() {
        renderThreadEvents.EventApplicationStageChange += applicationStageManager.ReceiveStageChange;

        renderThreadEvents.EventOpenGlContextCreated += (_, _) => { _openGlContextCreated.SetResult(true); };
        renderThreadEvents.EventOpenGlContextCreated += (_, _) => { Logger.Information("OpenGL Context Created"); };
        return this;
    }

    public IEngine SpawnRenderThread() {
        var renderThreadProcessor = EngineServices.CreateWithServices<RenderThreadProcessor>();
        renderThreadProcessor.CancellationToken = _ctsRenderThread.Token;
        renderThreadProcessor.TextureQueue = _textureQueue;
        var renderThread = new Thread(renderThreadProcessor.Run);
        renderThread.Start();
        Logger.Information("Spawned RenderThread");

        return this;
    }

    public async Task Run() {
        Logger.Information("Entered AterraEngine");

        // Don't wait forever for the opengl context to be created, else we will have many issues
        try {
            var cts = new CancellationTokenSource(5000);
            await _openGlContextCreated.Task.WithCancellation(cts.Token);
        }
        catch (OperationCanceledException ex) {
            Logger.Fatal(ex, "Render Thread did not signal back of successful window creation before the timeout period");
            await HandleFatalExceptionGracefully();
        }

        renderThreadEvents.InvokeApplicationStageChange(ApplicationStage.StartupScreen);

        foreach (AssetRegistration assetRegistration in pluginAtlas.GetAssetRegistrations()) {
            await Task.Delay(100); // TODO REMOVE DELAY
            if (!assetAtlas.TryAssignAsset(assetRegistration, out AssetId? _)) {
                Logger.Warning("Type {Type} could not be assigned as an asset", assetRegistration.Type);
            }
        }

        renderThreadEvents.InvokeApplicationStageChange(ApplicationStage.Level);
        
        // -------------------------------------------------------------------------------------------------------------
        TryAssignStartingLevel("NexitiesDebug:Levels/MainLevel");
        
        _textureQueue.Enqueue(new TextureQueueRecord (
            TextureAssetId :  "Workfloor:TextureDuckyHype"
        ));
        _textureQueue.Enqueue(new TextureQueueRecord (
            TextureAssetId :  "Workfloor:TextureDuckyPlatinum"
        ));

        const int a = 100;
        for (int k = 0; k < a; k++) {
            for (int j = 0; j < a; j++) {
                // AssetId assetId = "NexitiesDebug:Entities/DuckyPlatinumActor" ;
                AssetId assetId = j % 2 == 0 ? "Workfloor:ActorDuckyHype" : "Workfloor:ActorDuckyPlatinum";
                if (!instanceAtlas.TryCreate(assetId, out IActor2D? newDucky)) continue;
                newDucky.Transform2D.Translation = new Vector2(50 * j,50 * k);
                newDucky.Transform2D.Scale = new Vector2(50, 50);
                world.LoadedLevel?.AssetTree.AddLast(newDucky);
            }
        }
        
        await Task.Delay(50000000);
        await _ctsRenderThread.CancelAsync();
    }

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    private async Task HandleFatalExceptionGracefully() {
        await _ctsRenderThread.CancelAsync();

        Logger.ExitFatal((int)ExitCodes.GeneralError, "Fatally Crashing gracefully");
    }
}
