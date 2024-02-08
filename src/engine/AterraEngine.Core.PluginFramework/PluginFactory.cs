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

    private HashSet<Type> _assignedTypes = [];
    public IReadOnlyCollection<IPlugin> Plugins => _plugins;
    
    private ushort _pluginIdCounter;
    public ushort PluginIdCounter {
        get => _pluginIdCounter;
        set {
            if (value == ushort.MaxValue) {
                var maxId = ushort.MaxValue.ToString("X");
                EngineStartupLogger.Log.Fatal("Max Plugin Id of {maxId} is exhausted",maxId);
                throw new OverflowException($"Max Plugin Id of {maxId} is exhausted");
            }
            _pluginIdCounter = value;
        }
    }

    // -----------------------------------------------------------------------------------------------------------------
    // Constructor
    // -----------------------------------------------------------------------------------------------------------------

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    private bool _CheckPluginType(Type objectType) {
        if (!typeof(IPlugin).IsAssignableFrom(objectType)) {
            EngineStartupLogger.Log.Error(
                "Type {objectType} does not implement the IPlugin Interface and created as a Plugin.", objectType);
            return false;
        }
        
        if (!allowDuplicatePlugins && _assignedTypes.Contains(objectType)) {
            string assignedId = _plugins.FirstOrDefault(p => p.GetType() == objectType)?.Id.ToString() ?? "UNDEFINED";
            EngineStartupLogger.Log.Error(
                "Type {objectType} was already defined as a plugin by id {assignedId}", objectType, assignedId);
            return false;
        }

        return true;
    }
    
    public bool TryLoadPluginFromType(Type objectType) {
        if (!_CheckPluginType(objectType)) return false;
        var pluginId = new PluginId(PluginIdCounter++);
        EngineStartupLogger.Log.Information("New PluginId assigned: {pluginId}", pluginId);
        
        var plugin = (IPlugin)Activator.CreateInstance(objectType, args: pluginId)!;
        EngineStartupLogger.Log.Information("Plugin created: {plugin}", plugin);
        
        _plugins.AddLast(plugin);
        _assignedTypes.Add(objectType);
        return true;
    }
    
    public void LoadPluginsFromDLLFilePaths(IEnumerable<string> filePaths, bool throwOnFail = true) {
        foreach (string assemblyLocation in filePaths) {
            Assembly assembly = Assembly.LoadFrom(assemblyLocation);

            foreach (Type objectType in assembly.GetTypes()) {
                if (objectType is { IsInterface: true } or { IsAbstract: true }) continue;
                
                // Handle Engine Plugin
                if (!TryLoadPluginFromType(objectType)) {
                    EngineStartupLogger.Log.Warning(
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