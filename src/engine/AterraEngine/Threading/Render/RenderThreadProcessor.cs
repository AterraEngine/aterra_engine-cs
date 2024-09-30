// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Common.Attributes.DI;
using AterraCore.Common.Types.Nexities;
using AterraCore.Common.Types.Threading;
using AterraCore.Contracts.Nexities.Systems;
using AterraCore.Contracts.OmniVault.Textures;
using AterraCore.Contracts.OmniVault.World;
using AterraCore.Contracts.Renderer;
using AterraCore.Contracts.Threading;
using AterraCore.Contracts.Threading.CrossThread;
using AterraCore.Contracts.Threading.Logic;
using AterraCore.Contracts.Threading.Rendering;
using AterraCore.Contracts.Threading2.CrossData;
using AterraCore.Contracts.Threading2.CrossData.Holders;
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
    IWorldSpace world,
    ITextureAtlas textureAtlas,
    IThreadingManager threadingManager,
    IRenderEventManager eventManager,
    ILogicEventManager logicEventManager,
    ICrossThreadTickData crossThreadTickData,
    ICrossThreadDataAtlas crossThreadDataAtlas
) : IRenderThreadProcessor {
    private readonly Stack<Action> _endOfFrameActions = [];
    private bool _invokeCacheClear;
    
    private ILogger Logger { get; } = logger.ForContext<RenderThreadProcessor>();

    private static Color ClearColor { get; } = new(0, 0, 0, 0);
    public CancellationToken CancellationToken { get; set; }
    
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public void Run() {
        RegisterEvents();
        mainWindow.Init();

        // Window is actually running now
        while (!Raylib.WindowShouldClose()) {
            if (Raylib.IsWindowResized()) eventManager.InvokeWindowResized();
            logicEventManager.InvokeUpdateFps(Raylib.GetFPS());
            Update();
            HandleCrossThreadData();

            while (_endOfFrameActions.TryPop(out Action? action))
                action();
            
            if (_invokeCacheClear) {
                _invokeCacheClear = false;
                eventManager.InvokeClearSystemCaches();
            }
            
            crossThreadTickData.ClearOnRenderFrame();
        }

        Logger.Information("Render Thread Closing");
        Raylib.CloseWindow();
        threadingManager.CancelThreads();
    }

    public void RegisterEvents() {
        eventManager.EventClearSystemCaches += (_, _) => _endOfFrameActions.Push(OnEventManagerOnEventClearSystemCaches);
        eventManager.EventWindowResized += (_, _) => _endOfFrameActions.Push(OnEventManagerOnEventWindowResized);
    }
    
    private void Update() {
        if (world.ActiveLevel is not {
                Camera2DEntity: var camera2DEntity,
                RenderSystemsReversed: var renderSystemsReversed,
                UiSystems: var uiSystems
            } activeLevel) return;

        Raylib.BeginDrawing();
        Raylib.ClearBackground(ClearColor);

        if (camera2DEntity is { Camera: var camera2D }) {
            Raylib.BeginMode2D(camera2D);

            int count = renderSystemsReversed.Length;
            for (int i = count - 1; i >= 0; i--) 
                renderSystemsReversed[i].Tick(activeLevel);

            Raylib.EndMode2D();
        }

        foreach (INexitiesSystem uiSystem in uiSystems) {
            uiSystem.Tick(activeLevel);
        }
        
        Raylib.EndDrawing();
    }

    private void HandleCrossThreadData() {
        if (crossThreadDataAtlas.TryGetOrCreateTextureBus(out ITextureBus? textureBus) && !textureBus.IsEmpty) {
            while (textureBus.TexturesToLoad.TryDequeue(out AssetId id)) PushRegisterTexture(id);
            while (textureBus.TexturesToUnLoad.TryDequeue(out AssetId id)) PushUnRegisterTexture(id);
        }
        
        crossThreadDataAtlas.Try
    }

    private void PushRegisterTexture(AssetId id) {
        _endOfFrameActions.Push(RegisterTexture);
        return;

        void RegisterTexture() {
            textureAtlas.TryRegisterTexture(id);
            _invokeCacheClear = true;
        }
    }

    private void PushUnRegisterTexture(AssetId id) {
        _endOfFrameActions.Push(UnregisterTexture);
        return;

        void UnregisterTexture() {
            textureAtlas.TryUnRegisterTexture(id);
            _invokeCacheClear = true;
        }
    }

    // -----------------------------------------------------------------------------------------------------------------
    // Events
    // -----------------------------------------------------------------------------------------------------------------
    private void OnEventManagerOnEventClearSystemCaches() {
        if (world.ActiveLevel is not { RenderSystemsReversed: var renderSystemsReversed }) return;

        int count = renderSystemsReversed.Length;
        for (int i = count - 1; i >= 0; i--) {
            renderSystemsReversed[i].InvalidateCaches();
        }
    }

    private void OnEventManagerOnEventWindowResized() {
        if (world.ActiveLevel is not { Camera2DEntity: {} camera2DEntity }) return;

        camera2DEntity.Camera = camera2DEntity.Camera with {
            Offset = new Vector2(Raylib.GetScreenWidth() / 2f, Raylib.GetScreenHeight() / 2f)
        };
    }
}
