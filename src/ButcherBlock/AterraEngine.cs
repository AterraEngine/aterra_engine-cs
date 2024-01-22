// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Numerics;
using AterraEngine_lib.Config;
using AterraEngine.Config;
using AterraEngine.Interfaces.Assets;
using AterraEngine.Interfaces.Assets.Lib;
using AterraEngine.Interfaces.Component;
using AterraEngine.Interfaces.Plugin;
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
public class AterraEngine(IEnginePluginManager enginePluginManager) : IAtteraEngine{
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
        Raylib.SetConfigFlags(ConfigFlags.FLAG_WINDOW_RESIZABLE);
        
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
        if (!enginePluginManager.TryLoadOrderFromEngineConfig(engineConfig: _engineConfig, out _)) {
            throw new Exception("SOMETHING WENT WRONG!!!");
        }
        // Firstly, load all plugins from DLL files
        enginePluginManager.LoadPlugins();
        IServiceCollection serviceCollection = EngineServices.AssignDefaultServices(new ServiceCollection());
        
        // Load up all services from the plugins, the order is important
        enginePluginManager.DefinePluginServices(serviceCollection);
        
        // Only after everything is loaded, build the service provider
        EngineServices.BuildServiceProvider(serviceCollection);
        
        return true;
        
    }

    private bool TryLoadPluginData() {
        enginePluginManager.DefinePluginTextures();
        enginePluginManager.DefinePluginAssets();
        return true;
    }
    
    // -----------------------------------------------------------------------------------------------------------------
    // Main Loop
    // -----------------------------------------------------------------------------------------------------------------
    private int MainLoop() {
        IWorld2D world2D = EngineServices.GetWorld2D();
        IAssetAtlas assetAtlas = EngineServices.GetAssetAtlas();
        world2D.Level2D.ResolveAssetIds();
        
        assetAtlas.TryGetAsset(world2D.PlayerId, out var playerComponent);
        
        var worldToScreenSpace =  Raylib.GetWorldToScreen2D(Vector2.Zero, camera: camera);
        
        while (!Raylib.WindowShouldClose()) {
            ((IPlayerComponent)playerComponent!).LoadKeyMapping();
            
            // Your rendering or game loop logic goes here
            Raylib.BeginDrawing();
            Raylib.ClearBackground(Color.Blue);
            

            // Begin 2D drawing mode (camera)
            Raylib.BeginMode2D(camera);
            
            world2D.Draw(worldToScreenSpace);
            world2D.DrawDebug(worldToScreenSpace);
            world2D.Level2D.CollideAll();

            // End 2D drawing mode (camera)
            Raylib.EndMode2D();
            
            Raylib.DrawText(Raylib.GetFPS().ToString(), 100,100,200,Color.RED);
            
           // Draw your content here
            Raylib.EndDrawing();

            // camera.Target = ((IPlayerComponent)playerComponent!).Pos;

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