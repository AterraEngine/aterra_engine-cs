// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.DependencyInjection;
using AterraEngine.Frameworks.Continuum.Handlers;
using AterraEngine.Frameworks.Continuum.Hubs;
using AterraEngine.Frameworks.Continuum.PipelineSteps;

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
        hub.SubscribeHandler(handler);
            
        return this;
    }
    
    public ICommandBuilder<TCommand, TResult> WithPipelineSteps(params Type[] types) {
        ICommandPipelineStep<TCommand, TResult>[] pipelines = types
            .Select(type => provider.GetRequiredService(type.MakeGenericType(typeof(TCommand), typeof(TResult))))
            .Cast<ICommandPipelineStep<TCommand, TResult>>()
            .ToArray();
        
        hub.AddPipelines(pipelines);
        return this;
    }
}
