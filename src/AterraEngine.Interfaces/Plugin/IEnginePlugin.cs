// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Types;
using Microsoft.Extensions.DependencyInjection;

namespace AterraEngine.Interfaces.Plugin;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IEnginePlugin {
    public PluginId IdPrefix { get;}
    public string NameInternal { get;}
    public string NameReadable { get;}
    
    public IEnginePlugin DefineConfig(PluginId idPrefix);
    public void DefineServices(IServiceCollection serviceCollection);
    public void DefineTextures(); // static data (like sprites....)
    public void DefineAssets(); // static data (like sprites....)
}