// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

using AterraCore.Contracts.Renderer;

namespace AterraEngine.Renderer.Raylib;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------

public class MainWindow : IMainWindow{
    public int Width { get; } = 800;
    public int Height { get; } = 400;
    public string Name { get; } = "AterraEngine - Test";

    public bool IsInitialised { get; private set; }

    public void Init() {
        InitWindow(Width, Height, Name);

        IsInitialised = true;
    }
}