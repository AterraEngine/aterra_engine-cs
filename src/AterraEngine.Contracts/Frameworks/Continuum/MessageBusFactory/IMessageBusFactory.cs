// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.DependencyInjection.Services;

namespace AterraEngine.Frameworks.Continuum;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IMessageBusFactory : IFactoryService<IMessageBus> {
    ICommandBuilder<TCommand, TResult> AddCommand<TCommand, TResult>()
        where TCommand : ICommand<TResult>
        where TResult : struct;
    
    ITriggerBuilder<TTrigger> AddTrigger<TTrigger>() 
        where TTrigger : ITrigger;
}
