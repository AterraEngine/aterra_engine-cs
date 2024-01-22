// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Collections.ObjectModel;
using System.Reflection;
using AterraEngine.Interfaces.Plugin;
using AterraEngine.Types;
using Microsoft.Extensions.DependencyInjection;

namespace AterraEngine.EngineLoader.Plugins;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class EnginePluginManager: IEnginePluginManager {
    private readonly Dictionary<PluginId, IEnginePlugin> _enginePlugins = new();
    public ReadOnlyDictionary<PluginId, IEnginePlugin> EnginePlugins => _enginePlugins.AsReadOnly();
    
    // -----------------------------------------------------------------------------------------------------------------
    // Internal Methods
    // -----------------------------------------------------------------------------------------------------------------
    internal void LoadPlugins(IEnumerable<string> filePaths) {
        int pluginIdCounter = 0;
        
        foreach (string assemblyLocation in filePaths) {
            PluginId pluginId = new PluginId(pluginIdCounter++);
            
            Assembly assembly = Assembly.LoadFrom(assemblyLocation);

            foreach (var pluginType in assembly.GetTypes()) {
                // If at a later point, we want to add more type loading from these assemblies,
                //      simply expand this with more ifelse 
                if (!typeof(IEnginePlugin).IsAssignableFrom(pluginType)
                    || pluginType is not { IsInterface: false, IsAbstract: false }) continue;
                // This handles a lot of the basic setup of a plugin
                //      Services and other things shouldn't be defined in this config
                IEnginePlugin plugin = ((IEnginePlugin)Activator.CreateInstance(pluginType)!).DefineConfig(idPrefix: pluginId);
                _enginePlugins.Add(pluginId, plugin);
            }
        }
    }

    internal void LoadPluginServices(IServiceCollection serviceCollection) {
        foreach (var plugin in EnginePlugins.Values) {
            plugin.DefineServices(serviceCollection);
        }
    }

    internal void LoadPluginTextures() {
        foreach (var plugin in EnginePlugins.Values) {
            plugin.DefineTextures();
        }
    }

    internal void LoadPluginAssets() {
        foreach (var plugin in EnginePlugins.Values) {
            plugin.DefineAssets();
        }
    }


    // -----------------------------------------------------------------------------------------------------------------
    //  General Use-case Methods
    // -----------------------------------------------------------------------------------------------------------------
    public IEnumerable<IEnginePlugin> GetPluginsSorted(){
        return EnginePlugins.OrderBy(pair => pair.Key).Select(pair => pair.Value);
    }
}