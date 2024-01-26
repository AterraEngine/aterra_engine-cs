// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Contracts.Engine;
using AterraEngine.Contracts.EngineFactory.Config;
using AterraEngine.Contracts.WorldSpaces;
using Raylib_cs;

namespace AterraEngine;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class Engine2D(IWorldSpace2D worldSpace2D) : IEngine{
    public EngineConfigDto EngineConfig { get; private set; } = null!;

    public void ConfigureFromLoader(EngineConfigDto engineConfig) {
        EngineConfig = engineConfig;
    }

    public void Run() {
        // For data that has to be loaded before the main loop
        worldSpace2D.RunSetup();
        
        while (!Raylib.WindowShouldClose()) {
            // --- Start Logic Handling ---
            worldSpace2D.UpdateFrame();
            // --- End Logic Handling ---
            
            Raylib.BeginDrawing();
            // Begin 2D drawing mode (camera)
            // Console.WriteLine(worldSpace2D.Camera.Target);
            
            // --- Start Render World ---
            worldSpace2D.RenderFrameWorld();
            // --- End Rendering World ---
            
            // --- Start Render UI ---
            worldSpace2D.RenderFrameUi();
            // --- End Rendering UI ---
            
            Raylib.EndDrawing();
        }
        
        Raylib.CloseWindow();
        EngineServices.DisposeServiceProvider();
    }
}