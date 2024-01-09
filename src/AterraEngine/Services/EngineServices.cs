// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using Microsoft.Extensions.DependencyInjection;
using Serilog;

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
    public static ILogger GetLogger() => _serviceProvider.GetRequiredService<ILogger>();
}