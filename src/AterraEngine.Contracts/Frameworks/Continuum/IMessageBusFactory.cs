// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.DependencyInjection.Services;

namespace AterraEngine.Frameworks.Continuum;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IMessageBusFactory : IFactoryService<IMessageBus> {
    IMessageBusFactory AddCommand<TCommandHandler, TCommand, TResult>()
        where TCommand : ICommand<TResult>
        where TResult : struct
        where TCommandHandler : class, ICommandHandler<TCommand, TResult>;
}
