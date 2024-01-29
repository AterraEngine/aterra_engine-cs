// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.OldContracts.Factories;
using AterraEngine.Types;
using Microsoft.Extensions.DependencyInjection;

namespace AterraEngine.OldContracts.Plugin;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IEnginePlugin {
    PluginId Id { get; }
    string NameReadable { get;}
    
    Type PluginTextures  { get; }
    Type PluginAssets  { get; }

    void Define(PluginId pluginId);
    void DefineServices(IServiceCollection serviceCollection);
}