// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Core.Types;
using Microsoft.Extensions.DependencyInjection;

namespace AterraEngine.Contracts.Plugin;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public abstract class AEnginePlugin : IEnginePlugin  {
    public PluginId Id { get; private set; }

    public abstract string NameReadable { get; }
    public abstract Type PluginTextures { get; }
    public abstract Type PluginAssets { get; }
    
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public void Define(PluginId pluginId) {
        Id = pluginId;
        
        Console.WriteLine($"{Id} - {NameReadable}");
        
    }

    public void DefineServices(IServiceCollection serviceCollection) {}

}