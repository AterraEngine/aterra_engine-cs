// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace AterraEngine.Frameworks.Continuum;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IQueryBuilder<out TQuery, TResult>
    where TQuery : IQuery<TResult>
    where TResult : struct {


    IQueryBuilder<TQuery, TResult> WithHandler<TQueryHandler>() where TQueryHandler : class, IQueryHandler<TQuery, TResult>;
    IQueryBuilder<TQuery, TResult> WithPipelineSteps(params Type[] types) ;
}