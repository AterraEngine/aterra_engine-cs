// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

using System.Reflection;
using AterraCore.Contracts;
using AterraCore.Contracts.FlexiPlug;
using AterraCore.Loggers;
using AterraEngine.Configuration;
using AterraEngine.Renderer.RaylibCs;
using Microsoft.Extensions.DependencyInjection;
using AterraCore.Nexities.Lib.Components.Transform2D;
namespace Workfloor_AterraCore;

using AterraCore.Common.Data;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------

public static class Program {
    public static void Main(string[] args) {
        IEngine engine = new EngineConfiguration()
            .ImportEngineConfig(Paths.ConfigEngine)
            .SetEngineLogger(EngineLogger.CreateLogger)
            .AddCustomServices(new ServiceDescriptor(typeof(RaylibLogger), typeof(RaylibLogger),
                ServiceLifetime.Singleton))

            // Assigns services which may be overriden by plugins
            .AssignDefaultServices()
            .WithPluginConfiguration(pc => pc
                .ImportAssemblies(
                    new BareAssemblyPlugin(Assembly.GetEntryAssembly()!, "Workfloor-AterraCore", "AndreasSas"),
                    new BareAssemblyPlugin(Assembly.GetAssembly(typeof(Transform2D))!, "NexitiesLib", "AndreasSas")
                )
                .ImportPlugins()
                .AssignServices()
                .CreatePluginList()
            )

            // Assigns services which CAN NOT be overriden by plugins
            .AssignStaticServices()
            .BuildDependencyInjectionContainer()

            // Actually create the engine instance
            .CreateEngine();

        engine
            .SubscribeToEvents()
            .SpawnRenderThread()
            ;

        // Actually startup the engine
        Task.Run(engine.Run).GetAwaiter().GetResult();

    }
}