// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Collections.ObjectModel;
using System.Reflection;

using AterraEngine_lib.structs;
using AterraEngine_lib.Config;
using AterraEngine.Interfaces.Plugin;

namespace AterraEngine.Plugin;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class EnginePluginManager: IEnginePluginManager {
    private Dictionary<PluginId, string> _loadOrder = new();
    public ReadOnlyDictionary<PluginId, string> LoadOrder => _loadOrder.AsReadOnly();
    
    private Dictionary<PluginId, IEnginePlugin> _enginePlugins = new();
    public ReadOnlyDictionary<PluginId, IEnginePlugin> EnginePlugins => _enginePlugins.AsReadOnly();

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public bool TryLoadOrderFromEngineConfig(EngineConfig? engineConfig, out List<Tuple<string, string>> errorPaths) {
        errorPaths = [];
        errorPaths.AddRange(
            from engineConfigPlugin in engineConfig.Plugins 
            where !_loadOrder.TryAdd(engineConfigPlugin.Id, engineConfigPlugin.FilePath) 
            select new Tuple<string, string>(_loadOrder[engineConfigPlugin.Id], engineConfigPlugin.FilePath)
        );

        return errorPaths.Count == 0;
    }

    public void LoadPlugins() {
        if (_loadOrder.Count == 0) {
            throw new Exception("No plugins to load.");
        }
        
        var loadedPlugins = _loadOrder
            .OrderBy(pair => pair.Key)
            .Select(pair => (pair.Key, Assembly.LoadFrom(pair.Value)))
            .SelectMany(pair => {
                var (key, assembly) = pair;

                return assembly
                    .GetTypes()
                    .Where(type => typeof(IEnginePlugin).IsAssignableFrom(type)
                                   && !type.IsInterface
                                   && !type.IsAbstract
                    )
                    .Select(pluginType => {
                        IEnginePlugin plugin = (IEnginePlugin)Activator.CreateInstance(pluginType)!;
                        
                        // THIS HANDLES A LOT OF THE SETUP OF A PLUGIN
                        plugin.ManagedInitialize(idPrefix: key);

                        // TODO add Services with ServiceCollection

                        return (key, plugin);
                    });
            })
            .ToArray();
        
        // TODO, don't do this with a normal dictionary
        foreach (var plugin in loadedPlugins) {
            _enginePlugins.Add(plugin.key, plugin.plugin);
        }
    }
}