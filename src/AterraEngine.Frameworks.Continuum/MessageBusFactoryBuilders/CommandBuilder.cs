// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.DependencyInjection;
using AterraEngine.Frameworks.Continuum.Pipelines;

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
    public ICommandBuilder<TCommand, TResult> WithPipelineStep(Type type) {
        object requiredService = provider.GetRequiredService(type.MakeGenericType(typeof(TCommand), typeof(TResult)));
        if (requiredService is not ICommandPipelineStep<TCommand, TResult> pipeline) throw new InvalidOperationException("Failed to get pipeline step");
        
        hub.AddPipelineStep(pipeline);
        return this;
    }
    
    public ICommandBuilder<TCommand, TResult> WithPipelineStep<TCommandPipelineStep>() where TCommandPipelineStep : class, ICommandPipelineStep<TCommand, TResult> {
        var pipeline = provider.GetRequiredService<TCommandPipelineStep>();
        
        hub.AddPipelineStep(pipeline);
        return this;
    }
}
