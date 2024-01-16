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
        serviceCollection.AddSingleton<IPlayerController, PlayerControllerLooking>();
        serviceCollection.AddSingleton<ILevelComponent, DebugLevel>();
        
        return serviceCollection;
    }

    public override IEnginePlugin DefineData() {
        base.DefineData();
        
        ITextureAtlas textureAtlas = EngineServices.GetService<ITextureAtlas>();
        ISpriteAtlas spriteAtlas = EngineServices.GetService<ISpriteAtlas>();
        
        if (!textureAtlas.TryAddTexture("tDucky", "resources/DuckyHappy.png")) {
            throw new Exception("Texture could not be loaded");
        }
        if (!spriteAtlas.TryAddSprite("sDucky", "tDucky", out ISprite? sprite)) {
            throw new Exception("Sprite could not be added to atlas");
        }
        
        EngineServices.GetService<IPlayerController>().Sprite = sprite!;

        ILevelComponent level = EngineServices.GetService<ILevelComponent>();
        
        List<IActorComponent> drawableComponents = [];
        int counter = 0;
        
        for (int i = -5; i < 5; i++) {
            for (int j = -5; j < 5; j++) {
                spriteAtlas.TryAddSprite($"sDuckyBackground{i}{j}", "tDucky", out ISprite? spirteX);
                
                IActorComponent actor = EngineServices.GetService<IActorComponent>();
                
                actor.Pos = new Vector2((spirteX!.Texture.Width / 4f)*i, (spirteX!.Texture.Height / 4f)*j);
                actor.Rotation = (i * j) * 0.1f;
                actor.Sprite = spirteX!;
                drawableComponents.Add(actor);
                
                Console.WriteLine($"Actor {counter++} created");
            }
        }
        //
        // IActorComponent actor = EngineServices.GetService<IActorComponent>();
        // actor.Pos = new Vector2(250,250);
        // actor.Sprite = sprite2;
        // drawableComponents.Add(actor);
        
        
        drawableComponents.Add(EngineServices.GetService<IPlayerController>());
        level.DrawableComponents = drawableComponents.ToArray();
        
        return this;
    }
    
}