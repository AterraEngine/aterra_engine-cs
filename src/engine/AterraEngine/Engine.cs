// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

using AterraCore.Common.Nexities;
using AterraCore.Contracts;
using AterraCore.Contracts.FlexiPlug;
using AterraCore.Contracts.Nexities.Assets;
using AterraCore.Contracts.Nexities.Levels;
using AterraCore.Contracts.Nexities.Worlds;
using AterraCore.DI;
using AterraEngine.Threading;
using JetBrains.Annotations;
using Serilog;

namespace AterraEngine;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[UsedImplicitly]
public class Engine(ILogger logger, IAssetAtlas assetAtlas, IAssetInstanceAtlas instanceAtlas, IPluginAtlas pluginAtlas, IWorld world) : IEngine {
    
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public bool TryAssignStartingLevel(AssetId assetId) {
        return instanceAtlas.TryCreateInstance(assetId, out ILevel? level)
               && world.TryLoadLevel(level);
    }

    public void Run() {
        logger.Information("Entered AterraEngine");
        
        foreach (AssetRegistration assetRegistration in pluginAtlas.GetAssetRegistrations()) {
            if (!assetAtlas.TryAssignAsset(assetRegistration, out AssetId? _)) {
                logger.Warning("Type {Type} could not be assigned as an asset", assetRegistration.Type);
            }
        }
        
        // TODO create window etc...

        var cts = new CancellationTokenSource();
        
        var renderThreadProcessor = EngineServices.CreateWithServices<RenderThreadProcessor>();
        renderThreadProcessor.CancellationToken = cts.Token;

        var renderThread = new Thread(renderThreadProcessor.Run);
        renderThread.Start();
        
        // Task.Run(() => {
        //     Task.Delay(5000, cts.Token);
        //     cts.Cancel();
        // }, cts.Token);
    }
}