// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Numerics;
using AterraEngine_lib.structs;
using AterraEngine.Plugin;
using Ansi;
using AterraEngine.Interfaces.Draw;
using AterraEngine.Interfaces.Plugin;
using AterraEngine.Services;
using Microsoft.Extensions.DependencyInjection;
using Raylib_cs;

namespace EnginePlugin_Test;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class Plugin : EnginePlugin {
    public override IEnginePlugin DefineServices(IServiceCollection serviceCollection) {
        base.DefineServices(serviceCollection);
        return this;
    }

    public override IEnginePlugin DefineData() {
        base.DefineData();

        ISpriteAtlas spriteAtlas = EngineServices.GetService<ISpriteAtlas>();
        spriteAtlas.TryAddSprite("ducky", Raylib.LoadTexture("resources/DuckyHappy.png"), new Vector2(1,1));
        
        return this;
    }
    
}