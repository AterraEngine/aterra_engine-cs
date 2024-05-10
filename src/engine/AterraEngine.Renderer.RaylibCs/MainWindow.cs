// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

using AterraCore.Contracts.Renderer;

namespace AterraEngine.Renderer.RaylibCs;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------

public class MainWindow(RaylibLogger raylibLogger) : IMainWindow{
    private static int Width => 800;
    private static int Height => 400;
    private static string Name => "AterraEngine - Test";

    public bool IsInitialised { get; private set; }

    public void Init() {
        unsafe {
            // Necessary to write Raylib logs with Serilog
            SetTraceLogCallback(RaylibLogger.GetPointer());
        }
        
        InitWindow(Width, Height, Name);
        
        IsInitialised = true;
    }
}