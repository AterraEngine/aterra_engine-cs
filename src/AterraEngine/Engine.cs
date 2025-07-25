// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

using AterraEngine.Contracts;
using AterraEngine.Frameworks.Nexities;
using AterraEngine.Frameworks.Nexities.Library;
using Microsoft.Extensions.DependencyInjection;
using Raylib_cs;
using rlImGui_cs;

namespace AterraEngine;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public sealed class Engine : IEngine, IDisposable {
    public static Engine Instance { get; private set; } = null!;
    public IServiceProvider ServiceProvider { get; private set; } = null!;
    private static bool IsInitialized { get; set; }
    private bool _disposed;
    
    // -----------------------------------------------------------------------------------------------------------------
    // Constructors
    // -----------------------------------------------------------------------------------------------------------------
    private Engine() { }
    public static Engine Initialize(Action<IServiceCollection>? setupServices = null) {
        if (IsInitialized) throw new InvalidOperationException("Engine is already initialized");
        
        var services = new ServiceCollection();
        var engine = new Engine();
        Instance = engine;
        IEngine.Instance = Instance;
        
        services.AddSingleton(engine);
        services.AddSingleton<IEngine>(engine);
        
        services.RegisterServicesFromAterraEngine();
        services.RegisterServicesFromAterraEngineFrameworksNexities();
        services.RegisterServicesFromAterraEngineFrameworksNexitiesLibrary();
        
        setupServices?.Invoke(services);
        
        ServiceProvider serviceProvider = services.BuildServiceProvider();
        Instance.ServiceProvider = serviceProvider;
        
        IsInitialized = true;
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
        update ??= static () => { };
        
        while (!Raylib.WindowShouldClose()) {
            Raylib.BeginDrawing();
            Raylib.ClearBackground(Color.Black);
            
            rlImGui.Begin();
            
            update();
            
            rlImGui.End();
            Raylib.EndDrawing();
        }
        
        // Ensure we complete any pending ImGui frame before disposal
        Raylib.BeginDrawing();
        rlImGui.Begin();
        rlImGui.End();
        Raylib.EndDrawing();
    }
    
    public void Dispose() {
        if (_disposed) return;
        
        try {
            rlImGui.Shutdown();
        }
        catch (Exception ex) {
            // Log the exception but don't crash during shutdown
            Console.WriteLine($"Error during ImGui shutdown: {ex.Message}");
        }
        
        Raylib.CloseWindow();
        
        _disposed = true;
        GC.SuppressFinalize(this);
    }

        
}