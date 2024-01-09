// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine_lib.structs;
using AterraEngine.Plugin;
using Ansi;
using Microsoft.Extensions.DependencyInjection;

namespace EnginePlugin_Test;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class Plugin : EnginePlugin {
    public override void DefineServices(IServiceCollection service_collection) {
        base.DefineServices(service_collection);
        
    }
}