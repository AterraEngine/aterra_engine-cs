// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Interfaces.Atlases;
using AterraEngine.Types;
using Microsoft.Extensions.DependencyInjection;

namespace AterraEngine.Interfaces.Plugin;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IEnginePluginServices {
    public void Define(IServiceCollection serviceCollection); // static data (like sprites....)
}