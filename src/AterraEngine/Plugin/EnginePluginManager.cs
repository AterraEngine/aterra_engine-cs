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
    public IEnumerable<IEnginePlugin> GetPluginsSorted(){
        return EnginePlugins.OrderBy(pair => pair.Key).Select(pair => pair.Value);
    }
    
    public bool TryLoadOrderFromEngineConfig(EngineConfig engineConfig, out List<Tuple<string, string>> errorPaths) {
        errorPaths = [];
        errorPaths.AddRange(
            from engineConfigPlugin in engineConfig.Plugins 
            where !_loadOrder.TryAdd(engineConfigPlugin.Id, engineConfigPlugin.FilePath) 
            select new Tuple<string, string>(_loadOrder[engineConfigPlugin.Id], engineConfigPlugin.FilePath)
        );

        return errorPaths.Count == 0;
    }

    public void LoadPlugins(IServiceCollection serviceCollection) {

        IEnginePlugin defaultPlugin = new DefaultPlugin();
        defaultPlugin.DefineConfig(idPrefix: new PluginId(0));
        defaultPlugin.DefineServices(serviceCollection);
        
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
                        IEnginePlugin plugin = ((IEnginePlugin)Activator.CreateInstance(pluginType)!)
                            // THIS HANDLES A LOT OF THE SETUP OF A PLUGIN
                            .DefineConfig(idPrefix: key)
                            // Add Service mappings with ServiceCollection
                            .DefineServices(serviceCollection);
                        
                        Console.WriteLine(plugin.IdPrefix);
                        
                        return (key,plugin);
                    });
            })
            .ToArray();
        
        // TODO, don't do this with a normal dictionary
        // _enginePlugins.Add(new PluginId(0),defaultPlugin );
        
        foreach (var keyValue in loadedPlugins) {
            _enginePlugins.Add(keyValue.key, keyValue.Item2);
        }
    }
}