// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace AterraEngine.Frameworks.Continuum;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class QueryBuilder<TQuery, TResult> : IQueryBuilder<TQuery, TResult>
    where TQuery : IQuery<TResult>
    where TResult : struct
{
    public Type QueryType { get; } = typeof(TQuery);
    public Type ReturnType { get; } = typeof(TResult);
    
    public Type? QueryHandlerType { get; private set; }
    public Type[] PipelineSteps { get; private set; } = [];


    public IQueryBuilder<TQuery, TResult> WithHandler<TQueryHandler>() where TQueryHandler : class, IQueryHandler<TQuery, TResult> {
        QueryHandlerType = typeof(TQueryHandler);
        return this;
    }
    
    public IQueryBuilder<TQuery, TResult> WithPipelineSteps(params Type[] types) {
        PipelineSteps = PipelineSteps.Concat(types).ToArray();
        return this;
    }
}
