// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.DependencyInjection;
using System.Collections.Immutable;

namespace AterraEngine.Frameworks.Continuum;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class TriggerHubBuilder<TTrigger>(IServiceCollection serviceCollection) : ITriggerHubBuilder<TTrigger>
    where TTrigger : ITrigger {

    private ImmutableArray<Type> TriggerHandlerTypes { get; set; } = [];
    private ImmutableArray<Type> PipelineSteps { get; set; } = [];

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public ITriggerHubBuilder<TTrigger> WithHandler<TTriggerHandler>() where TTriggerHandler : class, ITriggerHandler<TTrigger> {
        TriggerHandlerTypes = TriggerHandlerTypes.Add(typeof(TTriggerHandler));
        serviceCollection.AddScoped<TTriggerHandler>();
        return this;
    }

    public ITriggerHubBuilder<TTrigger> WithPipelineSteps(params Type[] types) {
        PipelineSteps = [..PipelineSteps.Concat(types)];
        foreach (Type type in types) serviceCollection.AddScoped(type);
        return this;
    }
    
    public ITriggerHub BuildHub(IScopedProvider provider) {
        var hub = provider.GetRequiredService<ITriggerHub<TTrigger>>();

        // A trigger can have an unlimited amount of handlers
        // ReSharper disable once ForCanBeConvertedToForeach
        for (int i = 0; i < TriggerHandlerTypes.Length; i++) {
            Type triggerHandlerType = TriggerHandlerTypes[i];
            var handler = (ITriggerHandler<TTrigger>)provider.GetRequiredService(triggerHandlerType);
            hub.SubscribeHandler(handler);
        }

        // Add the pipelines to the hub
        var pipelines = new IPipelineStep<TTrigger, Task>[PipelineSteps.Length];

        for (int i = PipelineSteps.Length - 1; i >= 0; i--) {
            Type type = PipelineSteps[i];
            if (!type.IsGenericType) {
                pipelines[i] = (IPipelineStep<TTrigger, Task>)provider.GetRequiredService(type);
                continue;
            }

            Type genericType = type.GetGenericTypeDefinition();
            Type actualType = genericType.MakeGenericType(typeof(TTrigger));

            pipelines[i] = (IPipelineStep<TTrigger, Task>)provider.GetRequiredService(actualType);
        }
        hub.AddPipelines(pipelines);

        return hub;
    }
}
