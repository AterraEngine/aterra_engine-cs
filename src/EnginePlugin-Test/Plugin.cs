// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine;
using AterraEngine.Actors;
using AterraEngine.Plugins;
using AterraEngine.Types;
using Microsoft.Extensions.DependencyInjection;
using Raylib_cs;

namespace EnginePlugin_Test;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class Plugin: AbstractEnginePlugin {
    public override string NameInternal => "pluginTest";
    public override string NameReadable => "Test Plugin";

    public override void DefineServices(IServiceCollection serviceCollection) {
        
    }

    public override void DefineTextures() {
        
    }

    public override void DefineAssets() {
        var worldSpace2D = EngineServices.GetWorldSpace2D();
        worldSpace2D.Player2D = new PlayerControllerLooking(
            new EngineAssetId(IdPrefix, NextInternalId())
        );
        
        Texture2D texture2D = Raylib.LoadTexture("resources/DuckyHappy.png");
        Sprite sprite = new Sprite();
        sprite.Texture = texture2D;
        sprite.SelectionBox = new Rectangle(0,0, sprite.Texture.Width,sprite.Texture.Height);
    }
}