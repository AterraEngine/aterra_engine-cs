// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

using AterraCore.Common.Types.Nexities;
using AterraCore.Contracts.Nexities.Data.Assets;
using AterraCore.Contracts.OmniVault;
using AterraCore.Contracts.Renderer;
using AterraCore.DI;
using AterraCore.Nexities.Lib.Components.Sprite2D;
using AterraEngine.Renderer.RaylibCs.FrameProcessors;
using JetBrains.Annotations;

namespace AterraEngine.Renderer.RaylibCs;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------

[UsedImplicitly]
public class MainWindow(IApplicationStageManager applicationStageManager, IAssetInstanceAtlas instanceAtlas, ITextureAtlas textureAtlas) : IMainWindow {
    private static int Width => 800;
    private static int Height => 400;
    private static string Name => "AterraEngine - Test";

    public bool IsInitialised { get; private set; }

    public void Init() {
        unsafe {
            // Necessary to write Raylib logs with Serilog
            SetTraceLogCallback(RaylibLogger.GetPointer());
        }
        SetConfigFlags(ConfigFlags.ResizableWindow
        // | ConfigFlags.UndecoratedWindow
        // | ConfigFlags.MousePassthroughWindow
        );
        InitWindow(Width, Height, Name);
        SetWindowMonitor(1);// WArn dev stuff

        applicationStageManager.TryRegisterStage(
        ApplicationStage.Undefined,
        EngineServices.CreateWithServices<UndefinedRaylibFrameProcessor>()
        );

        applicationStageManager.TryRegisterStage(
        ApplicationStage.StartupScreen,
        EngineServices.CreateWithServices<StartupRaylibFrameProcessor>()
        );

        // TODO CHECK HOW I CAN DO THIS FROM THE MAIN THREAD
        // Guid id = Guid.Parse("827c3bc1-f688-4301-b342-b8958c1fe892");
        // if (instanceAtlas.TryCreateInstance(new AssetId("Nexities:Components/Sprite2D"), out Sprite2D? spriteInstance, id)) {
        //     // logger.Information("sprite created");
        //     spriteInstance.Texture2D = textureAtlas.LoadImage("assets/ducky-hype.png");
        //     // logger.Information("texture assigned");
        // }
        

        IsInitialised = true;
    }
}
