// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.DependencyInjection;

namespace AterraEngine.Frameworks.Continuum.DependencyInjectionExtensions;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public static class ServiceCollectionExtensions {
    public static IServiceCollection AddContinuum(this IServiceCollection serviceCollection, Action<IScopedProvider, IMessageBusFactory> configure) {
        serviceCollection.AddScoped(typeof(ICommandHub<,>), typeof(CommandHub<,>));
        serviceCollection.AddScoped(typeof(ITriggerHub<>), typeof(TriggerHub<>));
        serviceCollection.AddScoped(typeof(IQueryHub<,>), typeof(QueryHub<,>));

        serviceCollection.AddScopedFromFactory<IContinuum, IMessageBusFactory>();

        // The MessageBusFactory is a singleton service
        //      This is due to it holding its data once and then executing it when IContinuum is actually called
        serviceCollection.AddSingleton<IMessageBusFactoryConfiguration>(new ContinuumConfiguration(configure));
        serviceCollection.AddSingleton<IMessageBusFactory, MessageBusFactory>();

        return serviceCollection;
    }
}
