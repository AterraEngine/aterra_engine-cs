﻿// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Boot.Logic.PluginLoading;
using AterraCore.Common.Types.Nexities;
using AterraCore.Contracts.Boot.Logic.PluginLoading;
using AterraCore.Contracts.Boot.Operations;
using AterraCore.Loggers;
using System.Reflection;

namespace AterraCore.Boot.Operations;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class RegisterAssemblyAsPlugin<T>() : RegisterAssemblyAsPlugin(typeof(T).Assembly, new T().Enter()) where T : class, IAssemblyEntrypoint, new();
public class RegisterAssemblyAsPlugin(Assembly assembly, PluginId pluginId) : IBootOperation {
    private ILogger Logger { get; } = StartupLogger.CreateLogger(false).ForBootOperationContext<RegisterAssemblyAsPlugin>(); 

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public void Run(IBootComponents components) {
        Logger.Debug("Entered RegisterAssemblyAsPlugin for the assembly {name}", assembly.GetName().Name);
        components.AssemblyLoadedPlugins.AddLast(new AssemblyLoadedPluginDto(assembly, pluginId));
    }
}

