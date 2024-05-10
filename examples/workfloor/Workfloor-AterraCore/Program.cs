// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

using System.Reflection;
using AterraCore.Common;
using AterraCore.Contracts;
using AterraCore.Loggers;
using AterraEngine;
using AterraEngine.Configuration;
using AterraEngine.Renderer.RaylibCs;
using Microsoft.Extensions.DependencyInjection;
using Nexities.Lib.Components.Transform2D;

namespace Workfloor_AterraCore;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------

public static class Program {
    public static void Main(string[] args) {
        IEngine engine = new EngineConfiguration()
            .ImportEngineConfig(Paths.ConfigEngine)
            
            .SetEngineLogger(EngineLogger.CreateLogger)
            .AddCustomServices(new ServiceDescriptor(typeof(RaylibLogger), typeof(RaylibLogger), ServiceLifetime.Singleton))
            
            // Assigns services which may be overriden by plugins
            .AssignDefaultServices()
            
            .WithPluginConfiguration(pc => pc
                .ImportAssemblies(Assembly.GetEntryAssembly(), Assembly.GetAssembly(typeof(Transform2D)))
                .ImportPlugins()
                .AssignServices()
                .CreatePluginList()
            )
            
            // Assigns services which CAN NOT be overriden by plugins
            .AssignStaticServices()
            .BuildDependencyInjectionContainer()
            
            // Actually create the engine instance
            .CreateEngine();
        
        engine.Run();
    }
}