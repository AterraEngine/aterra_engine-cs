// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

using AterraCore.Contracts;
using AterraCore.Contracts.Nexities.Assets;
using AterraCore.DI;
using AterraCore.Nexities.Assets;
using AterraCore.Common;
using AterraCore.Loggers;
using AterraEngine.Core.Logging;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace AterraEngine;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------

public class EngineLoader {
    private ILogger _startupLogger = StartupLogger.CreateLogger();

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public IEngine Start() {
        
        
        var engineServiceBuilder = new EngineServiceBuilder(_startupLogger);
        
        // Services which may be overriden
        engineServiceBuilder.AssignDefaultServices([
            sc => sc.AddSingleton(EngineLogger.CreateLogger()),
        ]);
        
        // Load plugins
        
        // After plugins have been loaded
        //      - Finish up with assigning the Static Services (services which may not be overriden)
        //      - Build the actual EngineServices
        engineServiceBuilder.AssignStaticServices([
            sc => sc.AddSingleton<IAssetAtlas, AssetAtlas>(),
            sc => sc.AddSingleton<IEngine, Engine>()
        ]);

        engineServiceBuilder.FinishBuilding();
        
        // After this point all plugin data should be assigned
        
        // After this point the actual engine should start churning
        // Warn Quick and dirty for now
        return EngineServices.GetService<IEngine>();

    }
    
}