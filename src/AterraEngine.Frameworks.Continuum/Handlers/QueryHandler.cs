// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace AterraEngine.Frameworks.Continuum.Handlers;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public abstract class QueryHandler<TQuery, TResponse> : MessageHandler<TQuery, ValueTask<TResponse>>, IQueryHandler<TQuery, TResponse>
    where TQuery : IQuery<TResponse>
    where TResponse : struct;

