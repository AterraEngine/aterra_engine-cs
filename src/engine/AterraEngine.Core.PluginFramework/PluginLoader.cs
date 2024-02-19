// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Contracts.Core.PluginFramework;
using AterraEngine.Core.Types;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace AterraEngine.Core.PluginFramework;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class PluginLoader(ILogger startuplogger, IEnumerable<IPlugin> plugins) : IPluginLoader {
    public void AssignServices(IServiceCollection serviceCollection) {
        // First load all SERVICES
        foreach (IPlugin plugin in plugins) {
            plugin.AssignServices(serviceCollection);
            startuplogger.Information("Added Services from Plugin {pluginId}", plugin.Id);
        }
        startuplogger.Information("Finished importing services from plugins");
    }
    
    public void AssignAssets(){
        // Secondary load all the TEXTURES
        foreach (IPlugin plugin in plugins) {
            plugin.AssignAssets(startuplogger);
            startuplogger.Information("Added Assets from Plugin {pluginId}", plugin.Id);
        }
        startuplogger.Information("Finished importing assets from plugins");
    }
}