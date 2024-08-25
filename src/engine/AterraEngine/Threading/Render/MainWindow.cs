// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Contracts.Renderer;
using JetBrains.Annotations;
using Raylib_cs;

namespace AterraEngine.Threading.Render;
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
        unsafe {Raylib.SetTraceLogCallback(RaylibLogger.GetPointer());}
        
        Raylib.SetConfigFlags(
            ConfigFlags.ResizableWindow
            | ConfigFlags.Msaa4xHint // Enable Multi Sampling Anti Aliasing 4x (if available)
            // | ConfigFlags.InterlacedHint
            // | ConfigFlags.UndecoratedWindow
            // | ConfigFlags.MousePassthroughWindow
            // | ConfigFlags.TransparentWindow
        );
        
        Raylib.InitWindow(Width, Height, Name);
        // Raylib.SetWindowMonitor(1);// WArn dev stuff
        
        IsInitialised = true;
    }
}
