// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Interfaces.Engine;
using AterraEngine.Interfaces.EngineLoader.Config;
using AterraEngine.Interfaces.WorldSpaces;
using Raylib_cs;

namespace AterraEngine;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class Engine2D(IWorldSpace2D worldSpace2D) : IEngine{
    public IEngineConfig EngineConfig { get; private set; } = null!;

    public void ConfigureFromLoader(IEngineConfig engineConfig) {
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
            Raylib.ClearBackground(Color.Black);
            
            
            // Begin 2D drawing mode (camera)
            Raylib.BeginMode2D(worldSpace2D.Camera);
            
            // --- Start Render World ---
            worldSpace2D.RenderFrameWorld();
            // --- End Rendering World ---
            
            Raylib.EndMode2D();
            
            // --- Start Render UI ---
            worldSpace2D.RenderFrameUi();
            // --- End Rendering UI ---
            
            Raylib.EndDrawing();
        }
        
        Raylib.CloseWindow();
        EngineServices.DisposeServiceProvider();
    }
}