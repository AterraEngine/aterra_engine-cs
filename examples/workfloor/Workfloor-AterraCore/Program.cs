// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

using System.Reflection;
using AterraCore.Common;
using AterraCore.Contracts;
using AterraEngine;
using Nexities.Lib.Components.Transform2D;

namespace Workfloor_AterraCore;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------

public static class Program {
    public static void Main(string[] args) {
        IEngine engine = new EngineConfiguration()
            .ImportEngineConfig(Paths.ConfigEngine)
            .AssignDefaultServices()
            .ImportAssemblyAsPlugin(Assembly.GetEntryAssembly())
            .ImportAssemblyAsPlugin(Assembly.GetAssembly(typeof(Transform2D)))
            .ImportPlugins()
            
            // Manipulate services from plugins
            .PluginsAssignServices()
            
            // Assign Static Systems & build DI container
            .AssignStaticServices()
            .AssignDependencyInjectionContainer()
            
            // Actually create the engine instance
            .CreateEngine();
        
        engine.Run();
    }
}