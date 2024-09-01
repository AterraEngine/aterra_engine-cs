// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Boot.Logic.PluginLoading.Dto;
using AterraCore.Common.Types.Nexities;
using AterraCore.Contracts.Boot.Operations;
using AterraCore.Loggers;
using System.Reflection;

namespace AterraCore.Boot.Operations;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class RegisterAssemblyAsPlugin(Assembly assembly, PluginId pluginId) : IBootOperation {
    private ILogger Logger { get; } = StartupLogger.CreateLogger(false).ForBootOperationContext<RegisterAssemblyAsPlugin>();

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public void Run(IBootComponents components) {
        Logger.Debug("Entered RegisterAssemblyAsPlugin for the assembly {name}", assembly.GetName().Name);
        var pluginDto = new PluginBootDto();
        pluginDto.UpdateAssemblies([assembly]);
        pluginDto.PluginNameSpaceId = pluginId;

        components.AssemblyLoadedPlugins.AddLast(pluginDto);
    }
}
