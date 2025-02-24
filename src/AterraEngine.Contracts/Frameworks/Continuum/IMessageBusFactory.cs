// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.DependencyInjection.Services;

namespace AterraEngine.Frameworks.Continuum;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IMessageBusFactory : IFactoryService<IContinuum> {
    ICommandHubBuilder<TCommand, TResult> AddCommand<TCommand, TResult>()
        where TCommand : ICommand<TResult>
        where TResult : struct;

    ITriggerHubBuilder<TTrigger> AddTrigger<TTrigger>()
        where TTrigger : ITrigger;

    IQueryHubBuilder<TQuery, TResult> AddQuery<TQuery, TResult>()
        where TQuery : IQuery<TResult>
        where TResult : struct;
}
