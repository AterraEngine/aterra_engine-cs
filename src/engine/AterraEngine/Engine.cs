// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

using System.Runtime.CompilerServices;
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

public class Engine(ILogger logger, IPluginAtlas pluginAtlas, IAssetAtlas assetAtlas) : IEngine {
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public void Run() {
        logger.Information("Entered AterraEngine");
        
        // TODO create window etc...

        var cts = new CancellationTokenSource();
        
        var renderThreadProcessor = EngineServices.CreateWithServices<RenderThreadProcessor>();
        renderThreadProcessor.CancellationToken = cts.Token;

        var renderThread = new Thread(renderThreadProcessor.Run);
        renderThread.Start();
        
        assetAtlas.TryImportAssetsFromPlugins();

        Task.Run(() => {
            Task.Delay(5000, cts.Token);
            cts.Cancel();
        }, cts.Token);
    }
}