// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Contracts.PluginFramework;
using AterraEngine.Core.ServicesFramework;
using Microsoft.Extensions.DependencyInjection;

namespace AterraEngine.Core.PluginFramework;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class PluginLoader(IServiceCollection serviceCollection) : IPluginLoader {
    public void LoadPlugins(IReadOnlyCollection<IPlugin> plugins) {
        // First load all SERVICES
        foreach (var plugin in plugins) {
            plugin.Services?.LoadServices(serviceCollection);
            EngineLogger.Log.Information("Added Services from Plugin {pluginId}", plugin.Id);
        }
        EngineServices.BuildServiceProvider(serviceCollection);
        
        // Secondary load all the TEXTURES
        foreach (var plugin in plugins) {
            plugin.Textures?.LoadTextures();
            EngineLogger.Log.Information("Added Textures from Plugin {pluginId}", plugin.Id);
        }
        EngineLogger.Log.Information("Finished importing textures from plugins");
        
        // Third load all MODELS
        //  ... this doesn't exist yet in the 2d scope of the project
        
        // Fourth load all ASSETS
        foreach (var plugin in plugins) {
            plugin.Assets?.LoadAssets();
            EngineLogger.Log.Information("Added Assets from Plugin {pluginId}", plugin.Id);
        }
        EngineLogger.Log.Information("Finished importing assets from plugins");
    }
}