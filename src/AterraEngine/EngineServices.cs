// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Interfaces.Engine;
using AterraEngine.Interfaces.Atlases;
using AterraEngine.Interfaces.WorldSpaces;
using Microsoft.Extensions.DependencyInjection;

namespace AterraEngine;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
/// <summary>
/// A static class responsible for managing and providing services for the Aterra Engine.
/// </summary>
public static class EngineServices {
    private static ServiceProvider _serviceProvider = null!;
    public static ServiceProvider ServiceProvider => _serviceProvider;
    
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
    public static IAssetAtlas GetAssetAtlas() => _serviceProvider.GetRequiredService<IAssetAtlas>();
    public static ITexture2DAtlas GetTextureAtlas() => _serviceProvider.GetRequiredService<ITexture2DAtlas>();
    public static IWorldSpace2D GetWorldSpace2D() => _serviceProvider.GetRequiredService<IWorldSpace2D>();
    public static IEngine GetEngine() => _serviceProvider.GetRequiredService<IEngine>();
}