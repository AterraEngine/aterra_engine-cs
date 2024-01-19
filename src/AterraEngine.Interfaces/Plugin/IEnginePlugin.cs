// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine_lib.structs;
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
    public void DefineDataTextures(); // static data (like sprites....)
    public void DefineDataAssets(); // static data (like sprites....)
}