// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.DependencyInjection;
using AterraEngine.Frameworks.Continuum;
using AterraEngine.Frameworks.Continuum.Hubs;
using Workfloor.AterraEngine.Frameworks.Continuum.CommandHandlers;
using Workfloor.AterraEngine.Frameworks.Continuum.PipelineSteps;
using Workfloor.AterraEngine.Frameworks.Continuum.QueryHandlers;
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
        collection.AddTransient<SimpleQueryHandler>();
        
        // Needed for Continuum to work 
        collection.AddTransient(typeof(ICommandHub<,>), typeof(CommandHub<,>));
        collection.AddTransient(typeof(ITriggerHub<>), typeof(TriggerHub<>));
        collection.AddTransient(typeof(IQueryHub<,>), typeof(QueryHub<,>));
        collection.AddTransient(typeof(SimpleCommandPipelineStep<,>));
        collection.AddTransientFromFactory<IMessageBus, IMessageBusFactory>();
        collection.AddSingletonFromFactory<IMessageBusFactory>(static provider => {
            var factory = new MessageBusFactory(provider);

            factory.AddCommand<SimpleCommand, bool>()
                .WithHandler<SimpleCommandHandler>()
                .WithPipelineSteps(typeof(SimpleCommandPipelineStep<,>));
                // .WithPipelineStep<CommandPipelineStep<SimpleCommand, bool>>();
            
            factory.AddTrigger<SimpleTrigger>()
                .WithHandler<SimpleTriggerHandler>()
                .WithHandler<SimpleTriggerHandler2>();
            
            factory.AddQuery<SimpleQuery, bool>()
                .WithHandler<SimpleQueryHandler>();
            
            return factory;
        });
        
        await using IScopedProvider scopedProvider = collection.Build();
        var messageBus = scopedProvider.GetRequiredService<IMessageBus>();
        messageBus.StartProcessing(); // This currently means that all hub within the message bus are always running

        var doWhile = true;
        while (doWhile) {
            Console.Write("Enter a string: ");
            if (Console.ReadLine() is not {} input) continue;

            await Parallel.ForAsync(0, 10, CancellationToken.None, async (i, ct) => {
                try {
                    switch (input.ToLowerInvariant()) {
                        case "trigger" or "t": {
                            await messageBus.PublishAsync(new SimpleTrigger($"{input};;{i}", DateTime.UtcNow), ct);
                            break;
                        }

                        case "command" or "c": {
                            bool result = await messageBus.ExecuteAsync<SimpleCommand, bool>(new SimpleCommand($"{input};;{i}", DateTime.UtcNow), ct);
                            Console.WriteLine($"You entered: {input} and got : {result}");
                            break;
                        }

                        case "query" or "q" : {
                            bool queryResult = await messageBus.QueryAsync<SimpleQuery, bool>(new SimpleQuery($"{input};;{i}", DateTime.UtcNow), ct);
                            Console.WriteLine($"You entered: {input} and got : {queryResult}");
                            
                            break;
                        }

                        case "exit" or "x": {
                            doWhile = false;
                            break;
                        }
                    }
                }
                catch (OperationCanceledException ex) {
                    Console.WriteLine($"Operation canceled: {ex.Message}");
                }
            });
        }
        
        Console.WriteLine("Exiting...");
        return;
    }
}
