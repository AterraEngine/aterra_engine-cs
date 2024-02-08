// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using OldAterraEngine.Contracts.Atlases;
using OldAterraEngine.Contracts.DTOs.EngineConfig;
using OldAterraEngine.Contracts.Engine;
using OldAterraEngine.Contracts.Factories;
using OldAterraEngine.Contracts.WorldSpaces;
using OldAterraEngine.Core.Atlases;
using OldAterraEngine.Core.WorldSpaces;
using Microsoft.Extensions.DependencyInjection;
using OldAterraEngine.Contracts.Plugin;

namespace OldAterraEngine.Core.Factories;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class EngineFactory<TEngine>(string? pathToEngineConfig = null) where TEngine : class, IEngine {
    private string PathToEngineConfig { get; } = pathToEngineConfig ?? "resources/engine_config-example.xml";
    
    private readonly PluginFactory _pluginFactory = new();
    private readonly RaylibFactory _raylibFactory = new();
    private EngineConfigDto _engineConfigDto = null!;
    
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public TEngine CreateEngine() {
        
        Console.WriteLine($"{PathToEngineConfig} - {File.Exists(PathToEngineConfig)}");
        
        new EngineConfigFactory<EngineConfigDto>().TryLoadConfigFile(PathToEngineConfig, out EngineConfigDto? engineConfig);
        _engineConfigDto = engineConfig ?? EngineConfigDto.CreateDefault();
        
        // Only allow the plugins to be loaded from this project.
        _pluginFactory.LoadPlugins(_engineConfigDto.Plugins.Select(config => config.FilePath));
        
        // -- The following operations are meant to go in a specific order --
        CreateServiceProvider();
        
        _raylibFactory.CreateRaylibWindow(_engineConfigDto); // This needs to go after the service provider, but before any texture or asset loading
        
        _pluginFactory.LoadPluginTextures();
        _pluginFactory.LoadPluginAssets();
        
        // Finally create the Engine class, which will run everything
        IEngine engine = EngineServices.GetEngine();
        engine.ConfigureFromLoader(_engineConfigDto);
        
        return (TEngine)engine;
    }
    
    // -----------------------------------------------------------------------------------------------------------------
    // Default Services
    // -----------------------------------------------------------------------------------------------------------------
    private void CreateServiceProvider() {
        IServiceCollection serviceCollection = new ServiceCollection();
        
        // --- DEFAULT SERVICES ---
        serviceCollection.AddSingleton<IAssetAtlas, AssetAtlas>();
        serviceCollection.AddSingleton<ITexture2DAtlas, Texture2DAtlas>();

        serviceCollection.AddSingleton<ILevelFactory, LevelFactory>();
        serviceCollection.AddSingleton<IRaylibFactory, RaylibFactory>();
        
        serviceCollection.AddSingleton<IWorldSpace2D, WorldSpace2D>();
        serviceCollection.AddSingleton<IEngine, TEngine>(); // magic
        
        // --- LOAD SERVICES FROM PLUGINS ---
        foreach (IEnginePlugin? plugin in _pluginFactory.GetPluginsSorted()) {
            plugin.DefineServices(serviceCollection); // Might overload default services
        }
        
        // --- BUILD THE ACTUAL PROVIDER ---
        EngineServices.BuildServiceProvider(serviceCollection);
    }
}