// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

using AterraCore.Contracts;
using AterraCore.Contracts.Nexities.Assets;
using AterraCore.DI;
using AterraCore.Nexities.Assets;
using AterraCore.Common;
using AterraEngine.Core.Logging;
using Serilog;

namespace AterraEngine;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------

public class EngineLoader {
    private ILogger _startupLogger = StartupLoggerFactory.CreateLogger();

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public IEngine Start() {
        
        
        var engineServiceBuilder = new EngineServiceBuilder(_startupLogger);
        engineServiceBuilder.AssignDefaultServices();
        
        // Load plugins
        
        // After plugins have been loaded
        //      - Finish up with assigning the Static Services
        //      - Build the actual EngineServices
        engineServiceBuilder.AssignStaticServices((StaticService[])[
            StaticService.AsSingleton<IAssetAtlas, AssetAtlas>(),
            StaticService.AsSingleton<IEngine, Engine>()
        ]);
        engineServiceBuilder.FinishBuilding();
        
        // After this point all plugin data should be assigned
        
        // After this point the actual engine should start churning
        // Warn Quick and dirty for now
        return EngineServices.GetService<IEngine>();

    }
    
}