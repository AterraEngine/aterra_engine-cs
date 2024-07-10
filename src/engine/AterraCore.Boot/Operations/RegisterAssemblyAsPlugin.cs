// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Boot.Logic.PluginLoading;
using AterraCore.Common.Types.Nexities;
using AterraCore.Contracts.Boot.FlexiPlug;
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
    public void Run(IBootOperationComponents components) {
        Logger.Debug("Entered RegisterAssemblyAsPlugin for the assembly {name}", assembly.GetName().Name);
        components.AssemblyLoadedPlugins.AddLast(new AssemblyLoadedPluginDto(assembly, pluginId));
    }
}

public class RegisterAssemblyAsPlugin<T>() : RegisterAssemblyAsPlugin(typeof(T).Assembly, new T().Enter()) where T : class, IAssemblyEntrypoint, new();
