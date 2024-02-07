// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Reflection;
using AterraEngine.Contracts.PluginFramework;
using AterraEngine.Core.ServicesFramework;
using AterraEngine.Core.Types;

namespace AterraEngine.Core.PluginFramework;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class PluginFactory(bool allowDuplicatePlugins = false) : IPluginFactory {
    private LinkedList<IPlugin> _plugins = [];
    private ushort _pluginIdCounter; // PluginId's are ushorts
    private HashSet<Type> _assignedTypes = [];
    public IReadOnlyCollection<IPlugin> Plugins => _plugins;
    
    // -----------------------------------------------------------------------------------------------------------------
    // Constructor
    // -----------------------------------------------------------------------------------------------------------------

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    private bool _CheckPluginType(Type objectType) {
        if (!typeof(IPlugin).IsAssignableFrom(objectType)) {
            EngineLogger.Log.Error(
                "Type {objectType} does not implement the IPlugin Interface and created as a Plugin.", objectType);
            return false;
        }
        
        if (!allowDuplicatePlugins && _assignedTypes.Contains(objectType)) {
            var assignedId = _plugins.FirstOrDefault(p => p.GetType() == objectType)?.Id.ToString() ?? "UNDEFINED";
            EngineLogger.Log.Error(
                "Type {objectType} was already defined as a plugin by id {assignedId}", objectType, assignedId);
            return false;
        }

        return true;
    }
    
    public bool TryLoadPluginFromType(Type objectType) {
        if (!_CheckPluginType(objectType)) return false;
        PluginId pluginId = new PluginId(_pluginIdCounter++);
        EngineLogger.Log.Information("New PluginId assigned: {pluginId}", pluginId);
        
        IPlugin plugin = (IPlugin)Activator.CreateInstance(objectType, args: pluginId)!;
        EngineLogger.Log.Information("Plugin created: {plugin}", plugin);
        
        _plugins.AddLast(plugin);
        _assignedTypes.Add(objectType);
        return true;
    }
    
    public void LoadPluginsFromDLLFilePaths(IEnumerable<string> filePaths, bool throwOnFail = true) {
        foreach (string assemblyLocation in filePaths) {
            Assembly assembly = Assembly.LoadFrom(assemblyLocation);

            foreach (var objectType in assembly.GetTypes()) {
                if (objectType is { IsInterface: true } or { IsAbstract: true }) continue;
                
                // Handle Engine Plugin
                if (!TryLoadPluginFromType(objectType)) {
                    EngineLogger.Log.Warning(
                        "Type {objectType} does not implement the IPlugin Interface and skipped during plugin loading.",
                        objectType
                    );
                    if (throwOnFail) {
                        throw new InvalidOperationException("Plugin could not be loaded");
                    }
                } 
            }
        }
    }
}