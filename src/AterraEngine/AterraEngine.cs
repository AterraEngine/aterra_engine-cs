// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Config;
using AterraEngine.Plugin;
using Raylib_cs;

namespace AterraEngine;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class AterraEngine {
    private const string _engineConfigXml = "resources/engine_config-example.xml";
    
    private readonly EnginePluginManager _pluginManager = new();
    private EngineConfig _engineConfig = EngineConfig.GetDefault();
    
    // -----------------------------------------------------------------------------------------------------------------
    // Config
    // -----------------------------------------------------------------------------------------------------------------
    private bool TryLoadEngineConfig() {
        // EngineConfig Test
        EngineConfigParser<EngineConfig> configParser = new();
        
        if (!configParser.TryDeserializeFromFile(_engineConfigXml, out EngineConfig? engineConfig)
            || engineConfig is null) {
            throw new Exception("File coule not be parsed");
        }
        _engineConfig = engineConfig;
        
        return true;
    }
    
    private bool TryLoadRaylibConfig() {
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
        _pluginManager.LoadPlugins();
        return true;
        
    }
    
    private bool TryLoadPluginServices(){
        return true;
        
    }
    private bool TryLoadPluginFeatures(){
        return true;
        
    }
    
    // -----------------------------------------------------------------------------------------------------------------
    // Main Loop
    // -----------------------------------------------------------------------------------------------------------------
    private int MainLoop() {
        while (!Raylib.WindowShouldClose()) {
            // Your rendering or game loop logic goes here
            Raylib.BeginDrawing();
            Raylib.ClearBackground(Color.RAYWHITE);
            
            // Draw your content here
            Raylib.EndDrawing();

        }
        return 0;
    }
    
    // -----------------------------------------------------------------------------------------------------------------
    // Entry
    // -----------------------------------------------------------------------------------------------------------------
    public int Run() {
        if (!TryLoadEngineConfig()) {
            throw new Exception("Failed during loading of engine config data");
        }
        if (!TryLoadRaylibConfig()) {
            throw new Exception("Failed during loading of engine config data");
        }
        
        if (!TryLoadPlugins()) {
            throw new Exception("Failed during loading of plugin dll's");
        }
        
        if (!TryLoadPluginServices()) {
            throw new Exception("Failed during loading of plugin services");
        }
        
        if (!TryLoadPluginFeatures()) {
            throw new Exception("Failed during loading of plugins");
        }
        // Start up the main loop if everything is okay
        return MainLoop();
    }
}