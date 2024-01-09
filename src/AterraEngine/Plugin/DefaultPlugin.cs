// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Draw;
using AterraEngine.Interfaces.Draw;
using AterraEngine.Interfaces.Plugin;
using Microsoft.Extensions.DependencyInjection;

namespace AterraEngine.Plugin;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class DefaultPlugin : EnginePlugin{
    public override IEnginePlugin DefineServices(IServiceCollection serviceCollection) {
        base.DefineServices(serviceCollection);
        serviceCollection.AddTransient<ISprite, Sprite>();
        serviceCollection.AddSingleton<ISpriteAtlas, SpriteAtlas>();
        return this;
    }
}