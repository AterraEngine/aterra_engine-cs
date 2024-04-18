// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Types;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace AterraEngine.Contracts.Core.PluginFramework;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IPlugin {
    PluginId Id { get; }
    void AssignServices(IServiceCollection serviceCollection);
    void AssignAssets(ILogger startupLogger);
}