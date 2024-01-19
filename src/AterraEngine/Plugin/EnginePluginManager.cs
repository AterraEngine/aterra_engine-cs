// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Collections.ObjectModel;
using System.Reflection;

using AterraEngine_lib.structs;
using AterraEngine_lib.Config;
using AterraEngine.Interfaces.Plugin;

using Microsoft.Extensions.DependencyInjection;

namespace AterraEngine.Plugin;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class EnginePluginManager: IEnginePluginManager {
    private readonly Dictionary<PluginId, string> _loadOrder = new();
    public ReadOnlyDictionary<PluginId, string> LoadOrder => _loadOrder.AsReadOnly();
    
    private readonly Dictionary<PluginId, IEnginePlugin> _enginePlugins = new();
    public ReadOnlyDictionary<PluginId, IEnginePlugin> EnginePlugins => _enginePlugins.AsReadOnly();
    
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public bool TryLoadOrderFromEngineConfig(EngineConfig engineConfig, out List<string> errorPaths) {
        errorPaths = [];
        int pluginIdCounter = 0;
        
        foreach (var engineConfigPlugin in engineConfig.Plugins) {
            if (!_loadOrder.TryAdd(new PluginId(pluginIdCounter), engineConfigPlugin.FilePath)) {
                errorPaths.Add(engineConfigPlugin.FilePath);
            }
            pluginIdCounter++;
        }
        
        return errorPaths.Count == 0;
    }
    
    public void LoadPlugins() {
        foreach ((PluginId pluginId, string assemblyLocation) in GetLoadOrderSorted()) {
            Assembly assembly = Assembly.LoadFrom(assemblyLocation);

            foreach (var pluginType in assembly.GetTypes()) {
                // If at a later point, we want to add more type loading from these assemblies,
                //      simply expand this with more ifelse 
                if (typeof(IEnginePlugin).IsAssignableFrom(pluginType) 
                    && pluginType is { IsInterface: false, IsAbstract: false }
                ){
                    // This handles a lot of the basic setup of a plugin
                    //      Services and other things shouldn't be defined in this config
                    IEnginePlugin plugin = ((IEnginePlugin)Activator.CreateInstance(pluginType)!).DefineConfig(idPrefix: pluginId);
                    _enginePlugins.Add(pluginId, plugin);
                }
            }
        }
    }
    
    // -----------------------------------------------------------------------------------------------------------------
    //  General Use-case Methods
    // -----------------------------------------------------------------------------------------------------------------
    private IEnumerable<IEnginePlugin> GetPluginsSorted(){
        return EnginePlugins.OrderBy(pair => pair.Key).Select(pair => pair.Value);
    }
    private IOrderedEnumerable<KeyValuePair<PluginId, string>> GetLoadOrderSorted(){
        return _loadOrder.OrderBy(pair => pair.Key);
    }
    
    // -----------------------------------------------------------------------------------------------------------------
    // Final Loading of the plugin methods
    // -----------------------------------------------------------------------------------------------------------------
    public void DefinePluginServices(IServiceCollection serviceCollection) {
        foreach (var plugin in GetPluginsSorted()) {
            plugin.DefineServices(serviceCollection);
        }
    }
    public void DefinePluginTextures() {
        foreach (var plugin in GetPluginsSorted()) {
            plugin.DefineDataTextures();
        }
    }
    public void DefinePluginAssets() {
        foreach (var plugin in GetPluginsSorted()) {
            plugin.DefineDataAssets();
        }
    }
    
}