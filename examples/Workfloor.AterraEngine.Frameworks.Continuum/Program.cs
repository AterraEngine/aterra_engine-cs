// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.DependencyInjection;
using AterraEngine.Frameworks.Continuum;
using Workfloor.AterraEngine.Frameworks.Continuum.CommandHandlers;
using Workfloor.AterraEngine.Frameworks.Continuum.PipelineSteps;
using Workfloor.AterraEngine.Frameworks.Continuum.TriggerHandlers;

namespace Workfloor.AterraEngine.Frameworks.Continuum;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public static class Program {
    public static async Task Main() {
        var collection = new ServiceCollection();
        
        // Handlers
        collection.AddTransient<SimpleCommandHandler>();
        collection.AddTransient<SimpleTriggerHandler>();
        collection.AddTransient<SimpleTriggerHandler2>();
        
        // Needed for Continuum to work 
        collection.AddTransient(typeof(ICommandHub<,>), typeof(CommandHub<,>));
        collection.AddTransient(typeof(ITriggerHub<>), typeof(TriggerHub<>));
        collection.AddTransient(typeof(CommandPipelineStep<,>));
        collection.AddTransientFromFactory<IMessageBus, IMessageBusFactory>();
        collection.AddSingletonFromFactory<IMessageBusFactory>(static provider => {
            var factory = new MessageBusFactory(provider);

            factory.AddCommand<SimpleCommand, bool>()
                .WithHandler<SimpleCommandHandler>()
                .WithPipelineStep(typeof(CommandPipelineStep<,>))
                .WithPipelineStep<CommandPipelineStep<SimpleCommand, bool>>();
            
            factory.AddTrigger<SimpleTrigger>()
                .WithHandler<SimpleTriggerHandler>()
                .WithHandler<SimpleTriggerHandler2>();
            
            return factory;
        });
        
        await using IScopedProvider scopedProvider = collection.Build();
        var messageBus = scopedProvider.GetRequiredService<IMessageBus>();
        messageBus.StartProcessing(); // This currently means that all hub within the message bus are always running

        while (true) {
            Console.Write("Enter a string: ");
            if (Console.ReadLine() is not {} input) continue;
            await messageBus.PublishAsync(new SimpleTrigger(input, DateTime.UtcNow));
            bool result = await messageBus.ExecuteAsync<SimpleCommand, bool>(new SimpleCommand(input, DateTime.UtcNow));
            Console.WriteLine($"You entered: {input} and got : {result}");
        }
    }
}
