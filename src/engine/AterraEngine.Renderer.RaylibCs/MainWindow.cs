// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Contracts.Renderer;
using JetBrains.Annotations;

namespace AterraEngine.Renderer.RaylibCs;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[UsedImplicitly]
public class MainWindow : IMainWindow {
    private static int Width => 1000;
    private static int Height => 1000;
    private static string Name => "AterraEngine - Test";

    public bool IsInitialised { get; private set; }

    public void Init() {
        // Necessary to write Raylib logs with Serilog
        unsafe {SetTraceLogCallback(RaylibLogger.GetPointer());}
        
        SetConfigFlags(
            ConfigFlags.ResizableWindow
            | ConfigFlags.Msaa4xHint // Enable Multi Sampling Anti Aliasing 4x (if available)
            // | ConfigFlags.InterlacedHint
            // | ConfigFlags.UndecoratedWindow
            // | ConfigFlags.MousePassthroughWindow
            // | ConfigFlags.TransparentWindow
        );
        
        InitWindow(Width, Height, Name);
        // SetWindowMonitor(1);// WArn dev stuff
        
        IsInitialised = true;
    }
}
