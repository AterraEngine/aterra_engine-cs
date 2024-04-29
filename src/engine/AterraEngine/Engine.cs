// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

using AterraCore.Contracts;
using AterraCore.Contracts.FlexiPlug;
using AterraCore.Contracts.Nexities.Assets;
using Serilog;

namespace AterraEngine;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------

public class Engine(ILogger logger, IPluginAtlas pluginAtlas, IAssetAtlas assetAtlas) : IEngine {
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public void Run() {
        logger.Information("Entered AterraEngine");
        
        // TODO create window etc...

        assetAtlas.TryImportAssetsFromPlugins();
    }
}