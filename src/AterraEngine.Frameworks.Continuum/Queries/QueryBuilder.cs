// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.DependencyInjection;

namespace AterraEngine.Frameworks.Continuum;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class QueryBuilder<TQuery, TResult>(IQueryHub<TQuery, TResult> hub, IScopedProvider provider) : IQueryBuilder<TQuery, TResult>
    where TQuery : IQuery<TResult>
    where TResult : struct {

    public IQueryBuilder<TQuery, TResult> WithHandler<TQueryHandler>() where TQueryHandler : class, IQueryHandler<TQuery, TResult>{
        if (hub.HasSubscriptions) throw new InvalidOperationException("Cannot add handler after command hub has been populated");
        var handler = provider.GetRequiredService<TQueryHandler>(); 
        hub.SubscribeHandler(handler);
            
        return this;
    }
    
    public IQueryBuilder<TQuery, TResult> WithPipelineSteps(params Type[] types){
        IQueryPipelineStep<TQuery, TResult>[] pipelines = types
            .Select(type => provider.GetRequiredService(type.MakeGenericType(typeof(TQuery), typeof(TResult))))
            .Cast<IQueryPipelineStep<TQuery, TResult>>()
            .ToArray();
        
        hub.AddPipelines(pipelines);
        return this;
    }
}
