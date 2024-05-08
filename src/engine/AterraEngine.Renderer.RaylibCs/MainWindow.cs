// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

using AterraCore.Contracts.Renderer;

namespace AterraEngine.Renderer.RaylibCs;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------

public class MainWindow(RaylibLogger raylibLogger) : IMainWindow{
    public int Width { get; } = 800;
    public int Height { get; } = 400;
    public string Name { get; } = "AterraEngine - Test";

    public bool IsInitialised { get; private set; }

    public void Init() {
        unsafe {
            SetTraceLogCallback(raylibLogger.GetPointer());
        }
        
        InitWindow(Width, Height, Name);

        IsInitialised = true;
    }
}