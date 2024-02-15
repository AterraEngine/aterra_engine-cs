// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Contracts.Core.PluginFramework;
using Microsoft.Extensions.DependencyInjection;

namespace AterraEngine.Lib.Plugin;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public abstract class PluginServiceBuilder : IPluginServiceBuilder {
    public abstract void LoadServices(IServiceCollection serviceCollection);
}