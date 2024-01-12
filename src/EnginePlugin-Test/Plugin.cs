// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Numerics;
using AterraEngine.Plugin;
using AterraEngine.Draw;
using AterraEngine.Interfaces.Component;
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
    public override IServiceCollection DefineServices(IServiceCollection serviceCollection) {
        base.DefineServices(serviceCollection);
        serviceCollection.AddSingleton<IPlayerController, PlayerController>();
        serviceCollection.AddSingleton<ILevelComponent, DebugLevel>();
        
        return serviceCollection;
    }

    public override IEnginePlugin DefineData() {
        base.DefineData();

        ISpriteAtlas spriteAtlas = EngineServices.GetService<ISpriteAtlas>();
        
        if (!spriteAtlas.TryAddTexture("tDucky", "resources/DuckyHappy.png")) {
            throw new Exception("Texture could not be loaded");
        }
        if (!spriteAtlas.TryAddSprite("sDucky", "tDucky", out ISprite? sprite)) {
            throw new Exception("Sprite could not be added to atlas");
        }
        
        EngineServices.GetService<IPlayerController>().Sprite = sprite!;

        ILevelComponent level = EngineServices.GetService<ILevelComponent>();
        
        if (!spriteAtlas.TryAddSprite("sDuckyBackground", "tDucky", out ISprite? sprite2)) {
            throw new Exception("Sprite could not be added to atlas");
        }
        sprite2!.Tint = Color.GOLD;

        List<IActorComponent> drawableComponents = [];

        int counter = 0;
        
        for (int i = -250; i < 250; i++) {
            for (int j = -250; j < 250; j++) {
                IActorComponent actor = EngineServices.GetService<IActorComponent>();
                actor.Pos = new Vector2((sprite2.Texture.Width / 4f)*i, (sprite2.Texture.Height / 4f)*j);
                actor.Rotation = (i * j) * 0.1f;
                actor.Sprite = sprite2;
                drawableComponents.Add(actor);
                
                Console.WriteLine($"Actor {counter++} created");
            }
        }
        
        Console.WriteLine("Done Prepping");
        
        level.DrawableComponents = drawableComponents.ToArray();
        
        
        return this;
    }
    
}