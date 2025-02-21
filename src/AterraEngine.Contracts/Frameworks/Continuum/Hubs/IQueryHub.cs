// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Frameworks.Continuum.Handlers;

namespace AterraEngine.Frameworks.Continuum.Hubs;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IQueryHub {
    Task StartProcessingAsync();
}

public interface IQueryHub<TQuery, TOutput> : IQueryHub, IMessageHub<IMessageHandler<TQuery, ValueTask<TOutput>>, TQuery, ValueTask<TOutput>>
    where TQuery : IQuery<TOutput>
    where TOutput : struct ;