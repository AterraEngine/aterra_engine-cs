// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Common.Attributes.DI;
using AterraCore.Common.Types.Threading;
using AterraCore.Contracts.Nexities.Systems;
using AterraCore.Contracts.OmniVault.Textures;
using AterraCore.Contracts.OmniVault.World;
using AterraCore.Contracts.Renderer;
using AterraCore.Contracts.Threading;
using AterraCore.Contracts.Threading.CrossThread;
using AterraCore.Contracts.Threading.CrossThread.Dto;
using AterraCore.Contracts.Threading.Logic;
using AterraCore.Contracts.Threading.Rendering;
using JetBrains.Annotations;
using Serilog;
using System.Numerics;

namespace AterraEngine.Threading.Render;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[UsedImplicitly]
[Singleton<IRenderThreadProcessor>]
public class RenderThreadProcessor(
    ILogger logger,
    IMainWindow mainWindow,
    IAterraCoreWorld world,
    ITextureAtlas textureAtlas,
    ICrossThreadQueue crossThreadQueue,
    IThreadingManager threadingManager,
    IRenderEventManager eventManager,
    ILogicEventManager logicEventManager,
    ICrossThreadTickData crossThreadTickData
) : AbstractThreadProcessor, IRenderThreadProcessor {
    private readonly Stack<Action> _endOfFrameActions = [];
    private ILogger Logger { get; } = logger.ForContext<RenderThreadProcessor>();

    private static Color ClearColor { get; } = new(0, 0, 0, 0);
    //
    public event TickEventHandler? TickEvent2DMode;  
    public event TickEventHandler? TickEvent3DMode;  
    public event TickEventHandler? TickEventUiMode;  
    public event EmptyEventHandler? TickEventClearCaches;  

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    private bool WhileCondition => !CancellationToken.IsCancellationRequested || !Raylib.WindowShouldClose();
    public override void Run() {
        RegisterEventsStartup();
        mainWindow.Init();

        // Window is actually running now
        while (WhileCondition) {
            if (Raylib.IsWindowResized()) eventManager.InvokeWindowResized();
            logicEventManager.InvokeUpdateFps(Raylib.GetFPS());
            Update();
            HandleQueue();

            while (_endOfFrameActions.TryPop(out Action? action))
                action();

            crossThreadTickData.ClearOnRenderFrame();
        }

        Logger.Information("Render Thread Closing");
        Raylib.CloseWindow();
        threadingManager.CancelThreads();
    }
    public override void OnLevelChangeStarted(IActiveLevel oldLevel) {
        TickEvent2DMode = null;
        TickEvent3DMode = null;
        TickEventUiMode = null;
        TickEventClearCaches = null;
    }
    
    public override void OnLevelChangeCompleted(IActiveLevel newLevel) {
        RegisterTickEvents<IRender2DSystem>(ref TickEvent2DMode, newLevel.TryGetRender2DSystems, system => system.Render2DTick);
        RegisterTickEvents<IRender3DSystem>(ref TickEvent3DMode, newLevel.TryGetRender3DSystems, system => system.Render3DTick);
        RegisterTickEvents<IRenderUiSystem>(ref TickEventUiMode, newLevel.TryGetRenderUiSystems, system => system.RenderUITick);
        RegisterClearCacheEvents<IRenderClearableCacheSystem>(ref TickEventClearCaches, newLevel.TryGetRenderClearableCacheSystems, system => system.RenderThreadClearCaches);
    }


    public override void RegisterEventsStartup() {
        eventManager.EventClearSystemCaches += (_, _) => _endOfFrameActions.Push(() => TickEventClearCaches?.Invoke());
        eventManager.EventWindowResized += (_, _) => _endOfFrameActions.Push(OnEventManagerOnEventWindowResized);
    }
    
    private void Update() {
        if (world.ActiveLevel is not { Camera2DEntity: var camera2DEntity } activeLevel) return;

        Raylib.BeginDrawing();
        Raylib.ClearBackground(ClearColor);

        if (camera2DEntity is { Camera: var camera2D } ) {
            Raylib.BeginMode2D(camera2D);
            TickEvent2DMode?.Invoke(activeLevel);
            Raylib.EndMode2D();
        }

        TickEventUiMode?.Invoke(activeLevel);
        Raylib.EndDrawing();
    }
    
    private void HandleQueue() {
        while (crossThreadQueue.TextureRegistrarQueue.TryDequeue(out TextureRegistrar? textureRecord)) {
            if (textureRecord.UnRegister) PushUnRegisterTexture(textureRecord);
            else PushRegisterTexture(textureRecord);
        }

        if (crossThreadQueue.EntireQueueIsEmpty) return;

        while (crossThreadQueue.TryDequeue(QueueKey.LogicToRender, out Action? action))
            _endOfFrameActions.Push(action);
        while (crossThreadQueue.TryDequeue(QueueKey.MainToRender, out Action? action))
            _endOfFrameActions.Push(action);
    }

    private void PushRegisterTexture(TextureRegistrar textureRecord) {
        _endOfFrameActions.Push(RegisterTexture);
        return;

        void RegisterTexture() {
            textureAtlas.TryRegisterTexture(textureRecord.TextureAssetId);
            eventManager.InvokeClearSystemCaches();
        }
    }

    private void PushUnRegisterTexture(TextureRegistrar textureRecord) {
        _endOfFrameActions.Push(UnregisterTexture);
        return;

        void UnregisterTexture() {
            textureAtlas.TryUnRegisterTexture(textureRecord.TextureAssetId);
            eventManager.InvokeClearSystemCaches();
        }
    }

    // -----------------------------------------------------------------------------------------------------------------
    // Events
    // -----------------------------------------------------------------------------------------------------------------
    private void OnEventManagerOnEventWindowResized() {
        if (world.ActiveLevel is not { Camera2DEntity: {} camera2DEntity }) return;

        camera2DEntity.Camera = camera2DEntity.Camera with {
            Offset = new Vector2(Raylib.GetScreenWidth() / 2f, Raylib.GetScreenHeight() / 2f)
        };
    }
}
