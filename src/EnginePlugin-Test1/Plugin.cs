// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Numerics;
using AterraEngine_lib.structs;
using AterraEngine.Plugin;
using AterraEngine.Draw;
using AterraEngine.Interfaces.Assets;
using AterraEngine.Interfaces.Component;
using AterraEngine.Interfaces.Draw;
using AterraEngine.Interfaces.Plugin;
using AterraEngine.Services;
using Microsoft.Extensions.DependencyInjection;
using Raylib_cs;

namespace EnginePlugin_Test1;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class Plugin : EnginePlugin {
    public override string NameInternal { get; } = "Test1Plugin";
    public override string NameReadable { get; } = "Engine Plugin Test 1";

    public override void DefineServices(IServiceCollection serviceCollection) {}

    public override void DefineDataTextures() {}

    public override void DefineDataAssets() {
        var engineAssetId = NewEngineAssetId();
        Console.WriteLine(engineAssetId.PluginId);
        Console.WriteLine(engineAssetId.Id);
        Console.WriteLine(engineAssetId);

        if (EngineServices.GetAssetAtlas().TryGetAsset("0", out IAsset? asset)) {
            Console.WriteLine(asset?.InternalName);
        }

    }
    
}