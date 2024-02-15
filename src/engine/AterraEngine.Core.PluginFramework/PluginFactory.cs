// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Reflection;
using AterraEngine.Contracts.Core.PluginFramework;
using AterraEngine.Core.Types;
using Serilog;

namespace AterraEngine.Core.PluginFramework;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class PluginFactory(ILogger logger, bool allowDuplicatePlugins = false, int idStartsAt = 0) : IPluginFactory {
    private LinkedList<IPlugin> _plugins = [];

    private HashSet<Type> _assignedTypes = [];
    public IReadOnlyCollection<IPlugin> Plugins => _plugins;

    private ushort _pluginIdCounter = (ushort)idStartsAt;
    public ushort PluginIdCounter {
        get => _pluginIdCounter;
        set {
            if (value == ushort.MaxValue) {
                var maxId = ushort.MaxValue.ToString("X");
                logger.Fatal("Max Plugin Id of {maxId} is exhausted",maxId);
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
            logger.Error(
                "Type {objectType} does not implement the IPlugin Interface and created as a Plugin.", objectType);
            return false;
        }
        
        if (!allowDuplicatePlugins && _assignedTypes.Contains(objectType)) {
            string assignedId = _plugins.FirstOrDefault(p => p.GetType() == objectType)?.Id.ToString() ?? "UNDEFINED";
            logger.Error(
                "Type {objectType} was already defined as a plugin by id {assignedId}", objectType, assignedId);
            return false;
        }

        return true;
    }
    
    public bool TryLoadPluginFromType(Type objectType) {
        if (!_CheckPluginType(objectType)) return false;
        var pluginId = new PluginId(PluginIdCounter++);
        logger.Information("New PluginId assigned: {pluginId}", pluginId);
        
        var plugin = (IPlugin)Activator.CreateInstance(objectType, args: pluginId)!;
        logger.Information("Plugin created: {plugin}", plugin);
        
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
                    logger.Warning(
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