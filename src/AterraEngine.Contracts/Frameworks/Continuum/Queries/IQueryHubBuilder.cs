// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace AterraEngine.Frameworks.Continuum;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IQueryHubBuilder : IMessageHubBuilder<IQueryHub>;

public interface IQueryHubBuilder<out TQuery, TResult> : IQueryHubBuilder
    where TQuery : IQuery<TResult>
    where TResult : struct {


    IQueryHubBuilder<TQuery, TResult> WithHandler<TQueryHandler>() where TQueryHandler : class, IQueryHandler<TQuery, TResult>;
    IQueryHubBuilder<TQuery, TResult> WithPipelineSteps(params Type[] types) ;
}