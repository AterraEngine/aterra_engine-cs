// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Component;
using AterraEngine.Draw;
using AterraEngine.Interfaces.Component;
using AterraEngine.Interfaces.Draw;
using Microsoft.Extensions.DependencyInjection;

namespace AterraEngine.Plugin;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class DefaultPlugin : EnginePlugin{
    public override IServiceCollection DefineServices(IServiceCollection serviceCollection) {
        base.DefineServices(serviceCollection);
        
        serviceCollection.AddTransient<ISprite, Sprite>();
        serviceCollection.AddTransient<IActorComponent, ActorComponent>();
        serviceCollection.AddSingleton<ISpriteAtlas, SpriteAtlas>();
        serviceCollection.AddSingleton<ITextureAtlas, TextureAtlas>();
        
        Console.WriteLine("Default Plugin Services applied");
        
        return serviceCollection;
    }
}