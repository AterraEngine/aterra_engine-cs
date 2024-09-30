// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Common.Attributes.DI;
using AterraCore.Common.Types.Nexities;
using AterraCore.Contracts.Nexities.Systems;
using AterraCore.Contracts.OmniVault.Textures;
using AterraCore.Contracts.OmniVault.World;
using AterraCore.Contracts.Renderer;
using AterraCore.Contracts.Threading;
using AterraCore.Contracts.Threading.CrossData;
using AterraCore.Contracts.Threading.CrossData.Holders;
using AterraCore.Contracts.Threading.Rendering;
using JetBrains.Annotations;
using System.Numerics;

namespace AterraCore.Threading.Threads.Render;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[UsedImplicitly]
[Singleton<IRenderThreadProcessor>]
public class RenderThreadProcessor(
    IMainWindow mainWindow,
    IWorldSpace world,
    ITextureAtlas textureAtlas,
    IThreadingManager threadingManager,
    ICrossThreadDataAtlas crossThreadDataAtlas
) : AbstractThreadProcessor<RenderThreadProcessor>, IRenderThreadProcessor {
    private bool _invokeCacheClear;

    private static Color ClearColor { get; } = new(0, 0, 0, 0);

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public override void Run() {
        mainWindow.Init();

        // Window is actually running now
        while (!Raylib.WindowShouldClose()) {
            if (Raylib.IsWindowResized()) AddToEndOfTick(OnEventManagerOnEventWindowResized);
            crossThreadDataAtlas.DataCollector.Fps = Raylib.GetFPS();

            Update();
            HandleCrossThreadData();

            while (EndOfTickActions.TryPop(out Action? action))
                action();

            if (_invokeCacheClear) {
                _invokeCacheClear = false;
                OnEventManagerOnEventClearSystemCaches();
            }

            crossThreadDataAtlas.CleanupRenderTick();// Clear for the end of the tick
        }

        Logger.Information("Render Thread Closing");
        Raylib.CloseWindow();
        threadingManager.CancelThreads();
    }

    private void Update() {
        if (world.ActiveLevel is not {
                Camera2DEntity: var camera2DEntity,
                RenderSystems: var renderSystems,
                UiSystems: var uiSystems
            } activeLevel) return;

        Raylib.BeginDrawing();
        Raylib.ClearBackground(ClearColor);

        if (camera2DEntity is { Camera: var camera2D }) {
            Raylib.BeginMode2D(camera2D);

            foreach (IRenderSystem renderSystem in renderSystems) {
                renderSystem.Tick(activeLevel);
            }

            Raylib.EndMode2D();
        }

        foreach (IUiSystem uiSystem in uiSystems) {
            uiSystem.Tick(activeLevel);
        }

        Raylib.EndDrawing();
    }

    private void HandleCrossThreadData() {
        ITextureBus textureBus = crossThreadDataAtlas.TextureBus;
        if (!textureBus.IsEmpty) {
            while (textureBus.TexturesToLoad.TryDequeue(out AssetId id)) PushRegisterTexture(id);
            while (textureBus.TexturesToUnLoad.TryDequeue(out AssetId id)) PushUnRegisterTexture(id);
        }

        if (crossThreadDataAtlas.LevelChangeBus.IsLevelChangePending) {
            AddToEndOfTick(() => crossThreadDataAtlas.LevelChangeBus.IsLevelChangePending = false);
            _invokeCacheClear = true;
        }
    }

    private void PushRegisterTexture(AssetId id) {
        AddToEndOfTick(RegisterTexture);
        return;

        void RegisterTexture() {
            textureAtlas.TryRegisterTexture(id);
            _invokeCacheClear = true;
        }
    }

    private void PushUnRegisterTexture(AssetId id) {
        AddToEndOfTick(UnregisterTexture);
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
