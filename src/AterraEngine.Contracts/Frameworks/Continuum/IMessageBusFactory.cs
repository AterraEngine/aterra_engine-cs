// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.DependencyInjection.Services;

namespace AterraEngine.Frameworks.Continuum;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IMessageBusFactory : IFactoryService<IMessageBus> {
    IMessageBusFactory AddCommand<TCommand, TResult>() where TCommand : ICommand<TResult> where TResult : struct;
}
