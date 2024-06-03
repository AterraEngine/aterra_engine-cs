// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

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
            
            // --- Assign Services for the ServiceProvider ---
            // Assigns services which may be overriden by plugins
            .AssignDefaultServices([
                NewServiceDescriptor<RaylibLogger, RaylibLogger>(ServiceLifetime.Singleton),
                NewServiceDescriptor<IMainWindow, MainWindow>(ServiceLifetime.Singleton)
            ])
            
            // Run the logic needed by the sub configurations of Nexities, FlexiPlug, etc..
            .RunSubConfigurations()
            
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