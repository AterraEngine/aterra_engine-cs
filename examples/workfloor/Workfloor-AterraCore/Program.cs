// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

using AterraCore.Boot.FlexiPlug;
using AterraCore.Contracts;
using AterraCore.Contracts.Renderer;
using AterraEngine.Renderer.RaylibCs;
using Microsoft.Extensions.DependencyInjection;
using AterraEngine.Threading;
using AterraCore.Boot;
using AterraCore.Common.Data;
using static Extensions.ServiceDescriptorExtension;

namespace Workfloor_AterraCore;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------

public static class Program {
    public static void Main(string[] args) {
        IEngine engine = new EngineConfiguration()
            // Engine PluginDtos file also depends on the other configuration files
            .ImportEngineConfig(Paths.ConfigEngine)
            
            // --- Run all data from the sub configurations ---
            .WithSubConfiguration(configuration => configuration
                // FlexiPlug
                .ImportPlugins()
                .AssignServices()
            )
            
            // .WithSubConfiguration(configuration => configuration
            //     // Nexities
            // )
            
            // --- Assign Services for the ServiceProvider ---
            // Assigns services which may be overriden by plugins
            .AssignDefaultServices([
                NewServiceDescriptor<RaylibLogger>(ServiceLifetime.Singleton),
                NewServiceDescriptor<IMainWindow, MainWindow>(ServiceLifetime.Singleton)
            ])
            
            .AssignStaticServices([
                NewServiceDescriptor<RenderThreadEvents, RenderThreadEvents>(ServiceLifetime.Singleton),
                NewServiceDescriptor<IApplicationStageManager, ApplicationStageManager>(ServiceLifetime.Singleton),
            ])
            
            // Finish building the DI container
            .BuildDependencyInjectionContainer()

            
            // --- Create Engine ---
            // Actually create the engine instance
            .CreateEngine();
        
        // --- Engine is running ---
        engine
            .SubscribeToEvents()
            .SpawnRenderThread()
            ;

        // Actually startup the engine
        Task.Run(engine.Run).GetAwaiter().GetResult();

    }
}