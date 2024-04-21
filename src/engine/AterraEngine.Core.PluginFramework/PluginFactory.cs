// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

using System.Reflection;
using AterraEngine.Contracts.Core.PluginFramework;
using Serilog;

namespace AterraEngine.Core.PluginFramework;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class PluginFactory(ILogger logger, int idStartsAt = 0) : IPluginFactory {
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
    public bool TryLoadPluginFromType(Type objectType) {
        if (!_CheckObjectType<IPlugin>(objectType)) return false;
        var pluginId = new PluginId(PluginIdCounter++);
        logger.Information("New PluginId assigned: {pluginId}", pluginId);
        
        var plugin = (IPlugin)Activator.CreateInstance(objectType, args: pluginId)!;
        
        _plugins.AddLast(plugin);
        _assignedTypes.Add(objectType);
        return true;
    }

    private bool _CheckObjectType<T>(Type objectType) {
        if (typeof(T).IsAssignableFrom(objectType)) return true;

        logger.Error(
            "Type {objectType} does not implement the IPlugin Interface and created as a Plugin.", objectType);
        return false;
    }
    
    public void LoadPluginsFromDLLFilePaths(IEnumerable<string> filePaths, bool throwOnFail = true) {
        foreach (string assemblyLocation in filePaths) {
            Assembly assembly = Assembly.LoadFrom(assemblyLocation);

            foreach (Type objectType in assembly.GetTypes()) {
                if (objectType is { IsInterface: true } or { IsAbstract: true }) continue;
                
                var pluginId = new PluginId(PluginIdCounter++);
                logger.Information("New PluginId assigned: {pluginId}", pluginId);
                
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