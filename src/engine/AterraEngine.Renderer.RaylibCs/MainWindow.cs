// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

using AterraCore.Contracts.Renderer;
using AterraCore.DI;
using AterraEngine.Renderer.RaylibCs.FrameProcessors;

namespace AterraEngine.Renderer.RaylibCs;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------

public class MainWindow(RaylibLogger raylibLogger, IApplicationStageManager applicationStageManager) : IMainWindow{
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
        SetWindowMonitor(1); // WArn dev stuff
        
        applicationStageManager.TryRegisterStage(
            ApplicationStage.Undefined,
            EngineServices.CreateWithServices<UndefinedRaylibFrameProcessor>()
        );
        
        applicationStageManager.TryRegisterStage(
            ApplicationStage.StartupScreen,
            EngineServices.CreateWithServices<StartupRaylibFrameProcessor>()
        );
        
        IsInitialised = true;
    }
}