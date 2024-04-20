// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

using AterraCore.Contracts;
using AterraCore.DI;

namespace AterraEngine;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------

public class EngineLoader {

    public IEngine Start() {

        var engineServiceBuilder = new EngineServiceBuilder();
        engineServiceBuilder.AssignDefaultServices();
        
        // Load plugins
        
        // After plugins have been loaded
        //      - Finish up with assigning the Static Services
        //      - Build the actual EngineServices
        engineServiceBuilder.AssignStaticServices();
        engineServiceBuilder.FinishBuilding();
        
        // After this point all plugin data should be assigned
       
        
        // After this point the actual engine should start churning
        // Warn Quick and dirty for now
        return EngineServices.GetService<IEngine>();

    }
    
}