// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.DependencyInjection.Services;

namespace AterraEngine.Frameworks.Continuum;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IMessageBusFactory : IFactoryService<IContinuum> {
    ICommandBuilder<TCommand, TResult> AddCommand<TCommand, TResult>()
        where TCommand : ICommand<TResult>
        where TResult : struct;
    
    ITriggerBuilder<TTrigger> AddTrigger<TTrigger>() 
        where TTrigger : ITrigger;
    
    IQueryBuilder<TQuery, TResult> AddQuery<TQuery, TResult>()
        where TQuery : IQuery<TResult>
        where TResult : struct;
}
