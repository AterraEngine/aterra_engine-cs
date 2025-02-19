// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.DependencyInjection;

namespace AterraEngine.Frameworks.Continuum;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class CommandBuilder<TCommand, TResult>(ICommandHub<TCommand, TResult> hub, IScopedProvider provider) : ICommandBuilder<TCommand, TResult>
    where TCommand : ICommand<TResult>
    where TResult : struct 
{
    public ICommandBuilder<TCommand, TResult> WithHandler<TCommandHandler>() where TCommandHandler : class, ICommandHandler<TCommand, TResult> {
        if (hub.HasSubscriptions) throw new InvalidOperationException("Cannot add handler after command hub has been populated");
        var handler = provider.GetRequiredService<TCommandHandler>(); 
        hub.Subscribe(handler);
            
        return this;
    }
}
