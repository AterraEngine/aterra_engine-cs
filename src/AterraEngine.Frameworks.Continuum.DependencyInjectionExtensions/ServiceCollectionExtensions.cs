// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.DependencyInjection;

namespace AterraEngine.Frameworks.Continuum.DependencyInjectionExtensions;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public static class ServiceCollectionExtensions {
    public static IServiceCollection AddContinuum(this IServiceCollection serviceCollection, Action<IContinuumServiceFactory> configure) {
        serviceCollection.AddScoped(typeof(ICommandHub<,>), typeof(CommandHub<,>));
        serviceCollection.AddScoped(typeof(ITriggerHub<>), typeof(TriggerHub<>));
        serviceCollection.AddScoped(typeof(IQueryHub<,>), typeof(QueryHub<,>));

        serviceCollection.AddScopedFromFactory<IContinuum, IContinuumServiceFactory>();

        // The ContinuumServiceFactory is a singleton service
        //      This is due to it holding its data once and then executing it when IContinuum is actually called
        serviceCollection.WithContinuum(configure);
        serviceCollection.AddSingleton<IContinuumServiceFactory, ContinuumServiceFactory>();

        return serviceCollection;
    }

    private static IServiceCollection WithContinuum(this IServiceCollection serviceCollection, Action<IContinuumServiceFactory> configure) {
        serviceCollection.AddEnumerableSingleton<IContinuumConfiguration, ContinuumConfiguration>(new ContinuumConfiguration(configure));
        return serviceCollection;
    }
}
