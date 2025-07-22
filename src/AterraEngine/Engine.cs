// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

using AterraEngine.Frameworks.Nexities;
using Microsoft.Extensions.DependencyInjection;
using Raylib_cs;
using rlImGui_cs;

namespace AterraEngine;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public sealed class Engine : IDisposable {
    public static Engine Instance { get; private set; } = null!;
    public IServiceProvider ServiceProvider { get; private set; } = null!;
    private static bool IsInitialized { get; set; }
    
    // -----------------------------------------------------------------------------------------------------------------
    // Constructors
    // -----------------------------------------------------------------------------------------------------------------
    private Engine() { }
    public static Engine Initialize(Action<IServiceCollection>? setupServices = null) {
        if (IsInitialized) throw new InvalidOperationException("Engine is already initialized");
        
        var services = new ServiceCollection();
        services.AddSingleton(new Engine());
        services.RegisterServicesFromAterraEngine();
        services.RegisterServicesFromAterraEngineFrameworksNexities();
        
        setupServices?.Invoke(services);
        
        ServiceProvider serviceProvider = services.BuildServiceProvider();
        Instance = serviceProvider.GetRequiredService<Engine>();
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