// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace AterraEngine.Frameworks.Continuum;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IQueryHub : IMessageHub;

public interface IQueryHub<TQuery, TOutput> : IQueryHub, IMessageHub<IMessageHandler<TQuery, ValueTask<TOutput>>, TQuery, ValueTask<TOutput>>
    where TQuery : IQuery<TOutput>
    where TOutput : struct;
