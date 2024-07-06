﻿// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Common.Data;
using AterraCore.Common.Types.Nexities;
using AterraCore.Contracts;
using AterraCore.Contracts.FlexiPlug;
using AterraCore.Contracts.Nexities.Data.Assets;
using AterraCore.Contracts.Nexities.Data.Levels;
using AterraCore.Contracts.Nexities.Data.Worlds;
using AterraCore.Contracts.Renderer;
using AterraCore.DI;
using AterraCore.Loggers;
using AterraCore.Nexities.Lib.Entities.Actor;
using AterraEngine.Threading;
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
    IWorld world,
    RenderThreadEvents renderThreadEvents,
    IApplicationStageManager applicationStageManager
) : IEngine {
    private readonly CancellationTokenSource _ctsRenderThread = new();
    private readonly TaskCompletionSource<bool> _openGlContextCreated = new();
    private ConcurrentQueue<TextureQueueRecord> _textureQueue = new();
    private ILogger Logger { get; } = logger.ForEngineContext();
    
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public bool TryAssignStartingLevel(AssetId assetId) =>
        instanceAtlas.TryCreate(assetId, out ILevel? level)
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

        if (instanceAtlas.TryCreate(new AssetId("NexitiesDebug:Entities/DuckyHypeActor"), out Actor2D? instance, predefinedGuid:Guid.Parse("af15db3d-f69e-4382-a768-d163011125f5"))) {
            instance.Transform2D.Translation = new Vector2(150, 20);
            instance.Transform2D.Scale = new Vector2(100, 100);
            
            _textureQueue.Enqueue(new TextureQueueRecord (
                TextureAssetId : new AssetId("Nexities:Components/Sprite2D"),
                PredefinedGuid :  Guid.Parse("827c3bc1-f688-4301-b342-b8958c1fe892"),
                TexturePath : "assets/ducky-hype.png",
                RecordType : TextureQueueRecordType.Register
            ));
        }
        
        if (instanceAtlas.TryCreate(new AssetId("Nexities:Entities/Actor2D"), out Actor2D? instance2, predefinedGuid:Guid.Parse("af15db3d-f69e-4382-a768-123456789012"))) {
            instance2.Transform2D.Translation = new Vector2(20, 30);
            instance2.Transform2D.Scale = new Vector2(100, 100);
            
            _textureQueue.Enqueue(new TextureQueueRecord (
                TextureAssetId : new AssetId("Nexities:Components/Sprite2D"),
                PredefinedGuid :  instance2.Sprite2D.Guid,
                TexturePath : "assets/DuckyPlatinum.png",
                RecordType : TextureQueueRecordType.Register
            ));
        }

        // Task.Run(() => {
        //     Task.Delay(5000, cts.Token);
        //     _ctsRenderThread.Cancel();
        // }, _ctsRenderThread.Token);
    }

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    private async Task HandleFatalExceptionGracefully() {
        await _ctsRenderThread.CancelAsync();

        Logger.ExitFatal((int)ExitCodes.GeneralError, "Fatally Crashing gracefully");
    }
}
