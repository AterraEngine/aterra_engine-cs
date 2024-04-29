// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

using System.Reflection;
using AterraCore.Common;
using AterraCore.Contracts;
using AterraEngine;

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
            .ImportPlugins()
            
            // Manipulate services from plugins
            .PluginsAssignServices()
            
            // Assign Static Services & build DI container
            .AssignStaticServices()
            .AssignDependencyInjectionContainer()
            
            // Actually create the engine instance
            .CreateEngine();
        
        engine.Run();
    }
}