// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Reflection;
using OldAterraEngine.Contracts.Factories;
using OldAterraEngine.Contracts.Plugin;
using OldAterraEngine.Core.Types;

namespace OldAterraEngine.Core.Factories;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class PluginFactory {
    private readonly Dictionary<PluginId, IEnginePlugin> _enginePlugins = new();
    
    // -----------------------------------------------------------------------------------------------------------------
    // Load Plugin assemblies
    // -----------------------------------------------------------------------------------------------------------------
    public void LoadPlugins(IEnumerable<string> filePaths) {
        var pluginIdCounter = 0;
        
        foreach (string assemblyLocation in CheckLoadOrderForDuplicates(filePaths.ToArray())) {
            var pluginId = new PluginId(pluginIdCounter++);
            
            Assembly assembly = Assembly.LoadFrom(assemblyLocation);

            foreach (Type? objectType in assembly.GetTypes()) {
                // Handle Engine Plugin (ergo, data)
                if (typeof(IEnginePlugin).IsAssignableFrom(objectType)
                    && objectType is { IsInterface: false, IsAbstract: false }) {
                    
                    // This handles a lot of the basic setup of a plugin
                    //      Services and other things shouldn't be defined in this config
                    
                    var plugin = ((IEnginePlugin)Activator.CreateInstance(objectType)!);
                    plugin.Define(pluginId);
                    _enginePlugins.Add(pluginId, plugin);
                } 
            }
        }
    }
    
    // -----------------------------------------------------------------------------------------------------------------
    // Apply data from plugins
    // -----------------------------------------------------------------------------------------------------------------
    private static IPluginDataFactory LoadPluginDataFactory(PluginId pluginId, Type objectType) {
        var factory = EngineServices.CreateWithServices<IPluginDataFactory>(objectType);
        // IPluginDataFactory factory = (ActivatorUtilities.CreateInstance(EngineServices.ServiceProvider, objectType) as IPluginDataFactory)!;
        factory.Define(pluginId);
        return factory;

    }

    public void LoadPluginTextures() {
        foreach (IEnginePlugin? plugin in GetPluginsSorted()) {
            LoadPluginDataFactory(plugin.Id, plugin.PluginTextures).CreateData();
        }
    }

    public void LoadPluginAssets() {
        foreach (IEnginePlugin? plugin in GetPluginsSorted()) {
            LoadPluginDataFactory(plugin.Id, plugin.PluginAssets).CreateData();
        }
    }


    // -----------------------------------------------------------------------------------------------------------------
    //  General Use-case Methods
    // -----------------------------------------------------------------------------------------------------------------
    public IEnumerable<IEnginePlugin> GetPluginsSorted(){
        return _enginePlugins.AsReadOnly().OrderBy(pair => pair.Key).Select(pair => pair.Value);
    }

    private IEnumerable<string> CheckLoadOrderForDuplicates(string[] filePaths) {
        IEnumerable<string>? duplicates = filePaths
            .GroupBy(f => f)
            .Where(f => f.Count() > 1)
            .SelectMany(f => f);

        foreach (string? duplicate in duplicates) {
            throw new Exception($"Duplicate Plugin loading isn't allowed at {duplicate}");
        }

        return filePaths;
    }
}