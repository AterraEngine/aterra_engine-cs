// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

using ImGuiNET;
using Raylib_cs;
using rlImGui_cs;

namespace AterraEngine;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class Engine : IDisposable {
    private static readonly Engine Instance = new();
    // -----------------------------------------------------------------------------------------------------------------
    // Constructors
    // -----------------------------------------------------------------------------------------------------------------
    private Engine() { }
    public static Engine Initialize() {
        return Instance;
    }

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public void SetupWindow() {
        Raylib.InitWindow(800,400, "AterraEngine");
        
        Raylib.SetTargetFPS(60000000);
        
        rlImGui.Setup();
    }
    
    public void Run(Action? update = null) {
        Action onUpdate = update ?? (() => { });
        
        while (!Raylib.WindowShouldClose()) {
            Raylib.BeginDrawing();
            Raylib.ClearBackground(Color.Black);
            
            rlImGui.Begin();
            
            onUpdate();
            
            rlImGui.End();
            Raylib.EndDrawing();
        }
    }
    
    public void Dispose() {
        rlImGui.Shutdown();
        Raylib.CloseWindow();
        
        GC.SuppressFinalize(this);
    }
        
}