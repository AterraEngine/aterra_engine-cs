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
        foreach (IPlugin plugin in plugins) {
            if (plugin.Services is null) continue;
            
            plugin.Services.LoadServices(serviceCollection);
            EngineStartupLogger.Log.Information("Added Services from Plugin {pluginId}", plugin.Id);
        }
        EngineServices.BuildServiceProvider(serviceCollection);
        
        // Secondary load all the TEXTURES
        foreach (IPlugin plugin in plugins) {
            if (plugin.Textures is null) continue;
            
            plugin.Textures.LoadTextures();
            EngineStartupLogger.Log.Information("Added Textures from Plugin {pluginId}", plugin.Id);
        }
        EngineStartupLogger.Log.Information("Finished importing textures from plugins");
        
        // Third load all MODELS
        //  ... this doesn't exist yet in the 2d scope of the project
        
        // Fourth load all ASSETS
        foreach (IPlugin plugin in plugins) {
            if (plugin.Assets is null) continue;
            
            plugin.Assets.LoadAssets();
            EngineStartupLogger.Log.Information("Added Assets from Plugin {pluginId}", plugin.Id);
        }
        EngineStartupLogger.Log.Information("Finished importing assets from plugins");
    }
}