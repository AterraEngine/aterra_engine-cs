// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Assets;
using AterraEngine.Component;
using AterraEngine.Draw;
using AterraEngine.Interfaces.Assets;
using AterraEngine.Interfaces.Component;
using AterraEngine.Interfaces.Draw;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Events;

namespace AterraEngine.Services;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
/// <summary>
/// A static class responsible for managing and providing services for the Aterra Engine.
/// </summary>
public static class EngineServices {
    private static ServiceProvider _serviceProvider = null!;
    
    // -----------------------------------------------------------------------------------------------------------------
    // Methods  
    // -----------------------------------------------------------------------------------------------------------------
    /// <summary>
    /// Builds the service provider using the provided collection of services.
    /// </summary>
    /// <param name="serviceCollection">The collection of services to be used for building the service provider.
    /// Make sure to add necessary services to the collection before calling this method, typically after invoking one or more <see cref="EnginePlugin.defineEngineServices"/>.</param>
    public static void BuildServiceProvider(IServiceCollection serviceCollection) {
        _serviceProvider = serviceCollection.BuildServiceProvider();
    }
    public static void DisposeServiceProvider() {
        _serviceProvider.Dispose();
    }

    /// <summary>
    /// Retrieves a required service of type <typeparamref name="T"/> from the service provider.
    /// </summary>
    /// <typeparam name="T">The type of service to retrieve.</typeparam>
    /// <returns>The instance of the requested service.</returns>
    public static T GetService<T>() where T : notnull{
        return _serviceProvider.GetRequiredService<T>();
    }
    
    // -----------------------------------------------------------------------------------------------------------------
    // Quick Call Methods  
    // -----------------------------------------------------------------------------------------------------------------
    // public static ILogger GetLogger() => _serviceProvider.GetRequiredService<ILogger>();
    public static IAssetAtlas GetAssetAtlas() => _serviceProvider.GetRequiredService<IAssetAtlas>();
    
    // -----------------------------------------------------------------------------------------------------------------
    // Default Services
    // -----------------------------------------------------------------------------------------------------------------
    public static IServiceCollection AssignDefaultServices(IServiceCollection serviceCollection) {
        // TODO remove these two from the services
        serviceCollection.AddTransient<ISprite, Sprite>();
        serviceCollection.AddTransient<IActorComponent, ActorComponent>();
        
        serviceCollection.AddTransient<ISpriteAtlas, SpriteAtlas>();
        serviceCollection.AddSingleton<ITextureAtlas, TextureAtlas>();
        serviceCollection.AddSingleton<IAssetAtlas, AssetAtlas>();
        
        return serviceCollection;
    }
}