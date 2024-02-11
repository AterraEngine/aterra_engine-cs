// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Contracts.Core.PluginFramework;
using Microsoft.Extensions.DependencyInjection;

namespace AterraEngine.Lib.Plugin;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public abstract class PluginServicesFactory : IPluginServicesFactory {
    public abstract void LoadServices(IServiceCollection serviceCollection);
}