// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Collections.ObjectModel;
using AterraEngine_lib.structs;
using AterraEngine_lib.Config;
using Microsoft.Extensions.DependencyInjection;

namespace AterraEngine.Interfaces.Plugin;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IEnginePluginManager {
    public ReadOnlyDictionary<PluginId, string> LoadOrder { get; }
    public ReadOnlyDictionary<PluginId, IEnginePlugin> EnginePlugins { get; }

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public bool TryLoadOrderFromEngineConfig(EngineConfig engineConfig, out List<Tuple<string, string>> errorPaths);
    public void LoadPlugins(IServiceCollection serviceCollection);
}