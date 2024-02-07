// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using OldAterraEngine.Core.Types;
using Microsoft.Extensions.DependencyInjection;

namespace OldAterraEngine.Contracts.Plugin;

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