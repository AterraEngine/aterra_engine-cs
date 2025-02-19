// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Frameworks.Continuum.Pipelines;

namespace AterraEngine.Frameworks.Continuum;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface ICommandHub {
    ValueTask<T1> ExecuteAsync<T0,T1>(T0 commandData, CancellationToken ct = default) where T0 : ICommand<T1> where T1 : struct;
    Task StartProcessingAsync();
}

public interface ICommandHub<TCommand, TOutput> : ICommandHub, IMessageHub<TCommand, ValueTask<TOutput>>
    where TCommand : ICommand<TOutput>
    where TOutput : struct 
{
    void Subscribe<TCommandHandler>(TCommandHandler handler) where TCommandHandler : ICommandHandler<TCommand, TOutput>;
}