// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Numerics;
using AterraEngine_lib.Config;
using AterraEngine.Config;
using AterraEngine.Interfaces.Draw;
using AterraEngine.Plugin;
using AterraEngine.Services;
using Microsoft.Extensions.DependencyInjection;
using Raylib_cs;
using Serilog;
using Serilog.Events;
// using Serilog.Sinks.SystemConsole.Themes;

namespace AterraEngine;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class AterraEngine {
    private readonly EnginePluginManager _pluginManager = new();
    private EngineConfig _engineConfig = null!;
    
    // There can ONLY BE ONE!
    private static AterraEngine? _instance;
    public static AterraEngine Instance => _instance ??= new AterraEngine(); // Only get 

    // -----------------------------------------------------------------------------------------------------------------
    // Config
    // -----------------------------------------------------------------------------------------------------------------
    private bool TryLoadEngineConfig() {
        EngineConfigManager engineConfigManager = new EngineConfigManager("resources/engine_config-example.xml");
        engineConfigManager.TryLoadConfigFile(out _engineConfig, out _);
        // EngineConfig Test
        return true;
    }
    
    private bool TryLoadRaylibConfig() {
        Raylib.SetConfigFlags(ConfigFlags.FLAG_MSAA_4X_HINT);
        Raylib.InitWindow(
            _engineConfig.RaylibConfig.Window.Screen.Width, 
            _engineConfig.RaylibConfig.Window.Screen.Height, 
            _engineConfig.RaylibConfig.Window.Title
        );
        
        if (_engineConfig.RaylibConfig.Window.Icon != string.Empty) {
            // Load the image
            Image iconImage = Raylib.LoadImage(_engineConfig.RaylibConfig.Window.Icon);
            // apply the image
            Raylib.SetWindowIcon(iconImage);
            
            Raylib.UnloadImage(iconImage);
        }
        
        
        Raylib.InitAudioDevice();

        return true;
    }

    
    // -----------------------------------------------------------------------------------------------------------------
    // Loading Plugins
    // -----------------------------------------------------------------------------------------------------------------
    private bool TryLoadPlugins(){
        // Plugin Test
        if (!_pluginManager.TryLoadOrderFromEngineConfig(engineConfig: _engineConfig, out _)) {
            throw new Exception("SOMETHING WENT WRONG!!!");
        }
        
        IServiceCollection serviceCollection = new ServiceCollection();
        _pluginManager.LoadPlugins(serviceCollection);
        EngineServices.BuildServiceProvider(serviceCollection);
        
        return true;
        
    }

    private bool TryLoadPluginData() {
        foreach (var enginePlugin in _pluginManager.GetPluginsSorted()) {
            enginePlugin.DefineData();
        }
        
        return true;
    }
    
    // -----------------------------------------------------------------------------------------------------------------
    // Main Loop
    // -----------------------------------------------------------------------------------------------------------------
    private int MainLoop() {
        Vector2 velocity = new Vector2(.1f, .1f);
        Vector2 pos = Vector2.Zero;
        
        while (!Raylib.WindowShouldClose()) {
            if (Raylib.IsKeyDown(KeyboardKey.KEY_D))
                pos.X += velocity.X;
            if (Raylib.IsKeyDown(KeyboardKey.KEY_A))
                pos.X -= velocity.X;
            if (Raylib.IsKeyDown(KeyboardKey.KEY_W))
                pos.Y -= velocity.Y;
            if (Raylib.IsKeyDown(KeyboardKey.KEY_S))
                pos.Y += velocity.Y;
            
            // Your rendering or game loop logic goes here
            Raylib.BeginDrawing();
            Raylib.ClearBackground(Color.BLUE);
            
            ISpriteAtlas spriteAtlas = EngineServices.GetService<ISpriteAtlas>();
            spriteAtlas.TryGetSprite("ducky", out ISprite? sprite);
            
           if (sprite != null) sprite.Draw(pos);

            // Draw your content here
            Raylib.EndDrawing();

        }
        return 0;
    }
    
    // -----------------------------------------------------------------------------------------------------------------
    // Entry
    // -----------------------------------------------------------------------------------------------------------------
    public int Run() {
        IServiceCollection serviceCollection = new ServiceCollection();
        
        // These two have to be added before anything else
        //      Assigns logging and the Engine
        serviceCollection.AddSingleton<ILogger>(_ => {
            var logConfig = new LoggerConfiguration();
            logConfig.WriteTo.File(
                "log.log"
                    .Replace("{timestamp_iso8601}", DateTime.Now.ToString("yyyy-MM-ddTHH-mm-ss"))
                    .Replace("{timestamp_sortable}", DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss"))
                ,
                rollOnFileSizeLimit: true
            );
            
            logConfig.MinimumLevel.Is(LogEventLevel.Debug);
            return logConfig.CreateLogger();
        });
        
        if (!TryLoadEngineConfig()) {
            throw new Exception("Failed during loading of engine config data");
        }
        if (!TryLoadRaylibConfig()) {
            throw new Exception("Failed during loading of engine config data");
        }
        
        if (!TryLoadPlugins()) {
            throw new Exception("Failed during loading of plugin dll's");
        }
        
        if (!TryLoadPluginData()) {
            throw new Exception("Failed during loading of plugins");
        }
        // Start up the main loop if everything is okay
        return MainLoop();
    }
}