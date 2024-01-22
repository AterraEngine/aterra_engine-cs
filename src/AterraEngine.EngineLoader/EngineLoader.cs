// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.EngineLoader.Config;
using AterraEngine.EngineLoader.Plugins;
using AterraEngine.Interfaces.Engine;
using AterraEngine.Interfaces.EngineLoader;
using AterraEngine.Interfaces.Atlases;
using AterraEngine.Interfaces.WorldSpaces;
using AterraEngine.Atlases;
using AterraEngine.WorldSpaces;
using Microsoft.Extensions.DependencyInjection;
using Raylib_cs;

namespace AterraEngine.EngineLoader;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class EngineLoader<TEngine>(string? pathToEngineConfig = null) : IEngineLoader<TEngine> where TEngine : class, IEngine {
    private string PathToEngineConfig { get; } = pathToEngineConfig ?? "resources/engine_config-example.xml";
    
    private readonly IServiceCollection _serviceCollection = new ServiceCollection();
    private readonly EnginePluginManager _enginePluginManager = new();
    
    private EngineConfig _engineConfig = null!;
    
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public TEngine CreateEngine() {
        _engineConfig = new EngineConfigManager<EngineConfig>().LoadConfigFile(PathToEngineConfig);

        // Only allow the plugins to be loaded from this project.
        _enginePluginManager.LoadPlugins(_engineConfig.Plugins.Select(config => config.FilePath));
        
        // -- The following operations are meant to go in a specific order --
        AssignDefaultServices();
        _enginePluginManager.LoadPluginServices(_serviceCollection);
        EngineServices.BuildServiceProvider(_serviceCollection);
        
        InitRaylib();

        _enginePluginManager.LoadPluginTextures();
        _enginePluginManager.LoadPluginAssets();
        
        // Finally create the Engine class, which will run everything
        IEngine engine = EngineServices.GetEngine();
        engine.ConfigureFromLoader(
            _engineConfig
        );
        
        return (TEngine)engine;
    }
    
    // -----------------------------------------------------------------------------------------------------------------
    // Default Services
    // -----------------------------------------------------------------------------------------------------------------
    private void AssignDefaultServices() {
        
        // These three atlases are the based services which have to be defined at all times.
        _serviceCollection.AddSingleton<IAssetAtlas, AssetAtlas>();
        _serviceCollection.AddSingleton<IPluginAtlas, PluginAtlas>();
        _serviceCollection.AddSingleton<ITextureAtlas, TextureAtlas>();
        
        _serviceCollection.AddSingleton<IWorldSpace2D, WorldSpace2D>();
        _serviceCollection.AddSingleton<IEngine, TEngine>();

    }
    
    private void InitRaylib() {
        
        Raylib.InitWindow(
            _engineConfig.RaylibConfig.Window.Screen.Width, 
            _engineConfig.RaylibConfig.Window.Screen.Height, 
            _engineConfig.RaylibConfig.Window.Title
        );
        
        Raylib.SetConfigFlags(ConfigFlags.Msaa4xHint);
        Raylib.SetConfigFlags(ConfigFlags.ResizableWindow);
        
        if (_engineConfig.RaylibConfig.Window.Icon != string.Empty) {
            Image iconImage = Raylib.LoadImage(_engineConfig.RaylibConfig.Window.Icon);
            Raylib.SetWindowIcon(iconImage);
            Raylib.UnloadImage(iconImage);
        }
    }
}