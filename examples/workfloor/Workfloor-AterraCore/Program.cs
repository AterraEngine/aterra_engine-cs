// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

using AterraCore.Contracts;
using AterraCore.Contracts.Renderer;
using AterraEngine.Renderer.RaylibCs;
using Microsoft.Extensions.DependencyInjection;
using AterraEngine.Threading;
using AterraCore.Boot;
using AterraCore.Boot.Logic;
using AterraCore.Common.Data;
using static CodeOfChaos.Extensions.DependencyInjection.ServiceDescriptorExtension;

namespace Workfloor_AterraCore;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------

public static class Program {
    public static void Main(string[] args) {
        IEngine engine = new EngineConfiguration()
            .UseDefaultEngine()
            .ImportEngineConfig(Paths.ConfigEngine)
            
            // --- Assign SubConfigurations ---
            .AddSubConfigurations()
            .WithSubConfigurations(sc => {
                sc.FlexiPlug
                    .CheckAndIncludeRootAssembly() 
                    .PreLoadPlugins()
                ;
                
                // sc.Nexities
                // ;
            })
            
            // --- Assign Services for the ServiceProvider ---
            // Assigns services which may be overriden by plugins
            .AddDefaultServices([
                NewServiceDescriptor<RaylibLogger, RaylibLogger>(ServiceLifetime.Singleton),
                NewServiceDescriptor<IMainWindow, MainWindow>(ServiceLifetime.Singleton)
            ])
            
            .AddStaticServices([
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