// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.DependencyInjection;

namespace AterraEngine.Frameworks.Continuum.DependencyInjectionExtensions;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public static class ServiceCollectionExtensions {
    private static IContinuumServiceFactory? Factory { get; set; }
    public static IServiceCollection AddContinuum(this IServiceCollection serviceCollection, Action<IContinuumServiceFactory> configure) {
        serviceCollection.AddScoped(typeof(ICommandHub<,>), typeof(CommandHub<,>));
        serviceCollection.AddScoped(typeof(ITriggerHub<>), typeof(TriggerHub<>));
        serviceCollection.AddScoped(typeof(IQueryHub<,>), typeof(QueryHub<,>));

        serviceCollection.AddScopedFromFactory<IContinuum, IContinuumServiceFactory>();

        // The ContinuumServiceFactory is a singleton service
        //      This is due to it holding its data once and then executing it when IContinuum is actually called
        serviceCollection.WithContinuum(configure);

        return serviceCollection;
    }

    public static IServiceCollection WithContinuum(this IServiceCollection serviceCollection, Action<IContinuumServiceFactory> configure) {
        IContinuumServiceFactory factory = Factory ??= new ContinuumServiceFactory(serviceCollection);
        configure(factory);
        serviceCollection.AddSingleton(factory);

        return serviceCollection;
    }
}
