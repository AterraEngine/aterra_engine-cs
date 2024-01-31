// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Contracts.DTOs.EngineConfig;
using AterraEngine.Contracts.Engine;
using AterraEngine.Contracts.WorldSpaces;
using Raylib_cs;

namespace AterraEngine.Core;

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

        // --- Start Logic Handling ---
        Thread thread = new(worldSpace2D.RunLogic);
        thread.Start();
        
        while (!Raylib.WindowShouldClose()) {
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