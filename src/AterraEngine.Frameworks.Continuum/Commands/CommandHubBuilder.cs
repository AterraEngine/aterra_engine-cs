// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.DependencyInjection;
using System.Collections.Immutable;

namespace AterraEngine.Frameworks.Continuum;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class CommandHubBuilder<TCommand, TResult>(IServiceCollection serviceCollection) : ICommandHubBuilder<TCommand, TResult>
    where TCommand : ICommand<TResult>
    where TResult : struct {

    private Type CommandHandlerType { get; set; } = null!;
    private ImmutableArray<Type> PipelineSteps { get; set; } = [];

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public ICommandHubBuilder<TCommand, TResult> WithHandler<TCommandHandler>() where TCommandHandler : class, ICommandHandler<TCommand, TResult> {
        CommandHandlerType = typeof(TCommandHandler);
        serviceCollection.AddScoped<TCommandHandler>();
        return this;
    }

    public ICommandHubBuilder<TCommand, TResult> WithPipelineSteps(params Type[] types) {
        PipelineSteps = [..PipelineSteps.Concat(types)];
        foreach (Type type in types) serviceCollection.AddScoped(type);
        return this;
    }

    public ICommandHub BuildHub(IScopedProvider provider) {
        var hub = provider.GetRequiredService<ICommandHub<TCommand, TResult>>();
        // A command has just one handler
        if (CommandHandlerType == null) throw new InvalidOperationException("No command handler specified");
        var handler = (ICommandHandler<TCommand, TResult>)provider.GetRequiredService(CommandHandlerType);
        hub.SubscribeHandler(handler);

        // Add the pipelines to the hub
        var pipelines = new IPipelineStep<TCommand, ValueTask<TResult>>[PipelineSteps.Length];

        for (int i = PipelineSteps.Length - 1; i >= 0; i--) {
            Type type = PipelineSteps[i];
            if (!type.IsGenericType) {
                pipelines[i] = (IPipelineStep<TCommand, ValueTask<TResult>>)provider.GetRequiredService(type);
                continue;
            }

            Type genericType = type.GetGenericTypeDefinition();
            Type actualType = genericType.MakeGenericType(typeof(TCommand), typeof(TResult));

            pipelines[i] = (IPipelineStep<TCommand, ValueTask<TResult>>)provider.GetRequiredService(actualType);
        }

        hub.AddPipelines(pipelines);

        return hub;
    }
}
