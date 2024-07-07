﻿// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Contracts;
using AterraCore.Boot;
using AterraCore.Boot.Operations;
using AterraCore.Contracts.Boot;
using static AterraCore.Common.Data.PredefinedAssetIds.NewBootOperationNames;

namespace Workfloor_AterraCore;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public static class Program {
    public static void Main(string[] args) {
        INewEngineConfiguration config = new NewEngineConfiguration()
            .RegisterBootOperation(new RegisterWarnings(), after: EngineConfigLoaderOperation)
            .RegisterBootOperation(new EngineConfigLoader())
            .RegisterBootOperation(new CollectDependencies(), after: RegisterWarningsOperation)
            .RegisterBootOperation(new BuildDependencies(), after: CollectDependenciesOperation)
        ;
        
        config.RunBootOperations();

        IEngine engine = config.BuildEngine();
        Task.Run(engine.Run)
            .GetAwaiter()
            .GetResult();


        // IEngine engine = new EngineConfiguration()
        //     .UseDefaultEngine()
        //     .ImportEngineConfig(Paths.ConfigEngine)
        //     
        //     // --- Assign SubConfigurations ---
        //     .WithSubConfigurations(sc => {
        //         // Has to be ran before FlexiPlug configuration.
        //         //      Else it will add the simulated plugin after other plugins
        //         sc.Nexities
        //             .IncludeNexitiesLibAssembly();
        //         
        //         sc.FlexiPlug
        //             .CheckAndIncludeRootAssembly() 
        //             .PreLoadPlugins()
        //         ;
        //     })
        //     
        //     // --- Assign Services for the ServiceProvider ---
        //     // Assigns services which may be overriden by plugins
        //     .AddDefaultServices([
        //         NewServiceDescriptor<RaylibLogger, RaylibLogger>(ServiceLifetime.Singleton),
        //         NewServiceDescriptor<IMainWindow, MainWindow>(ServiceLifetime.Singleton)
        //     ])
        //     
        //     .AddStaticServices([
        //         NewServiceDescriptor<RenderThreadEvents, RenderThreadEvents>(ServiceLifetime.Singleton),
        //         NewServiceDescriptor<IApplicationStageManager, ApplicationStageManager>(ServiceLifetime.Singleton),
        //     ])
        //     
        //     // Finish building the DI container
        //     .BuildDependencyInjectionContainer()
        //     
        //     // --- Create Engine ---
        //     // Actually create the engine instance
        //     .CreateEngine();
        //
        // // --- Engine is running ---
        // engine
        //     .SubscribeToEvents()
        //     .SpawnRenderThread()
        // ;
        //
        // // Actually startup the engine
        // // Task.Run(engine.Run).GetAwaiter().GetResult();

    }
}