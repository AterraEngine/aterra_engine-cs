// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

using AterraCore.Common.Nexities;
using AterraCore.Contracts;
using AterraCore.Contracts.FlexiPlug;
using AterraCore.Contracts.Nexities.Assets;
using AterraCore.DI;
using AterraEngine.Threading;
using Serilog;

namespace AterraEngine;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------

public class Engine(ILogger logger, IAssetAtlas assetAtlas, IPluginAtlas pluginAtlas) : IEngine {
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
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