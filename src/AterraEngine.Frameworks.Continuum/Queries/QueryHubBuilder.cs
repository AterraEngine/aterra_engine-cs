// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.DependencyInjection;
using System.Collections.Immutable;

namespace AterraEngine.Frameworks.Continuum;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class QueryHubBuilder<TQuery, TResult>(IServiceCollection serviceCollection) : IQueryHubBuilder<TQuery, TResult>
    where TQuery : IQuery<TResult>
    where TResult : struct {
    private Type QueryHandlerType { get; set; } = null!;
    private ImmutableArray<Type> PipelineSteps { get; set; } = [];

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public IQueryHubBuilder<TQuery, TResult> WithHandler<TQueryHandler>() where TQueryHandler : class, IQueryHandler<TQuery, TResult> {
        QueryHandlerType = typeof(TQueryHandler);
        serviceCollection.AddScoped<TQueryHandler>();
        return this;
    }

    public IQueryHubBuilder<TQuery, TResult> WithPipelineSteps(params Type[] types) {
        PipelineSteps = [..PipelineSteps.Concat(types)];
        foreach (Type type in types) serviceCollection.AddScoped(type);
        return this;
    }

    public IQueryHub BuildHub(IScopedProvider provider) {
        var hub = provider.GetRequiredService<IQueryHub<TQuery, TResult>>();
        if (QueryHandlerType == null) throw new InvalidOperationException("No command handler specified");

        // A query can have just one handler
        var handler = (IQueryHandler<TQuery, TResult>)provider.GetRequiredService(QueryHandlerType);
        hub.SubscribeHandler(handler);

        // Add the pipelines to the hub
        var pipelines = new IPipelineStep<TQuery, ValueTask<TResult>>[PipelineSteps.Length];

        for (int i = PipelineSteps.Length - 1; i >= 0; i--) {
            Type type = PipelineSteps[i];
            if (!type.IsGenericType) {
                pipelines[i] = (IPipelineStep<TQuery, ValueTask<TResult>>)provider.GetRequiredService(type);
                continue;
            }

            Type genericType = type.GetGenericTypeDefinition();
            Type actualType = genericType.MakeGenericType(typeof(TQuery), typeof(TResult));

            pipelines[i] = (IPipelineStep<TQuery, ValueTask<TResult>>)provider.GetRequiredService(actualType);
        }

        hub.AddPipelines(pipelines);

        return hub;
    }
}
