// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using OldAterraEngine.Contracts.Atlases;
using OldAterraEngine.Contracts.Engine;
using OldAterraEngine.Contracts.WorldSpaces;
using Microsoft.Extensions.DependencyInjection;

namespace OldAterraEngine.Core;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
/// <summary>
/// A static class responsible for managing and providing services for the Aterra Engine.
/// </summary>
public static class EngineServices {
    public static ServiceProvider ServiceProvider { get; private set; } = null!;

    // -----------------------------------------------------------------------------------------------------------------
    // Methods  
    // -----------------------------------------------------------------------------------------------------------------
    /// <summary>
    /// Builds the service provider using the provided collection of services.
    /// </summary>
    /// <param name="serviceCollection">The collection of services to be used for building the service provider.
    /// Make sure to add necessary services to the collection before calling this method, typically after invoking one or more <see cref="EnginePlugin.defineEngineServices"/>.</param>
    public static void BuildServiceProvider(IServiceCollection serviceCollection) {
        ServiceProvider = serviceCollection.BuildServiceProvider();
    }
    public static void DisposeServiceProvider() {
        ServiceProvider.Dispose();
    }

    /// <summary>
    /// Retrieves a required service of type <typeparamref name="T"/> from the service provider.
    /// </summary>
    /// <typeparam name="T">The type of service to retrieve.</typeparam>
    /// <returns>The instance of the requested service.</returns>
    public static T GetService<T>() where T : notnull{
        return ServiceProvider.GetRequiredService<T>();
    }

    public static T CreateWithServices<T>(Type objectType) {
        return (T)ActivatorUtilities.CreateInstance(ServiceProvider, objectType);
    }
    public static T CreateWithServices<T>(){
        return (T)ActivatorUtilities.CreateInstance(ServiceProvider, typeof(T));
    }
    
    // -----------------------------------------------------------------------------------------------------------------
    // Quick Call Methods  
    // -----------------------------------------------------------------------------------------------------------------
    public static IAssetAtlas       GetAssetAtlas()     => ServiceProvider.GetRequiredService<IAssetAtlas>();
    public static ITexture2DAtlas   GetTexture2DAtlas()   => ServiceProvider.GetRequiredService<ITexture2DAtlas>();
    public static IWorldSpace2D     GetWorldSpace2D()   => ServiceProvider.GetRequiredService<IWorldSpace2D>();
    public static IEngine           GetEngine()         => ServiceProvider.GetRequiredService<IEngine>();
}