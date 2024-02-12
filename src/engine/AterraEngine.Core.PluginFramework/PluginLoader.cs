// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Contracts.Core.PluginFramework;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace AterraEngine.Core.PluginFramework;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class PluginLoader(ILogger logger, IEnumerable<IPlugin> plugins) : IPluginLoader {
    public void AssignPluginServices(IServiceCollection serviceCollection) {
        // First load all SERVICES
        foreach (IPlugin plugin in plugins) {
            if (plugin.Services is null) continue;

            plugin.Services.LoadServices(serviceCollection);
            logger.Information("Added Services from Plugin {pluginId}", plugin.Id);
        }
    }
    
    public void LoadPluginData(){
        // Secondary load all the TEXTURES
        foreach (IPlugin plugin in plugins) {
            if (plugin.Textures is null) continue;
            
            plugin.Textures.LoadTextures();
            logger.Information("Added Textures from Plugin {pluginId}", plugin.Id);
        }
        logger.Information("Finished importing textures from plugins");
        
        // Third load all MODELS
        //  ... this doesn't exist yet in the 2d scope of the project
        
        // Fourth load all ASSETS
        foreach (IPlugin plugin in plugins) {
            if (plugin.Assets is null) continue;
            
            plugin.Assets.LoadAssets();
            logger.Information("Added Assets from Plugin {pluginId}", plugin.Id);
        }
        logger.Information("Finished importing assets from plugins");
    }
}