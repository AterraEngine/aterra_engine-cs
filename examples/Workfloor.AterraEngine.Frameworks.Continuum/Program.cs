// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.DependencyInjection;
using AterraEngine.Frameworks.Continuum;
using AterraEngine.Frameworks.Continuum.DependencyInjectionExtensions;
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
        collection.AddTransient(typeof(SimpleCommandPipelineStep<,>));
        
        // Needed for Continuum to work 
        collection.AddContinuum(static (_, factory) => {
            factory.AddCommand<SimpleCommand, bool>()
                .WithHandler<SimpleCommandHandler>()
                .WithPipelineSteps(typeof(SimpleCommandPipelineStep<,>));
            // .WithPipelineStep<CommandPipelineStep<SimpleCommand, bool>>();

            factory.AddTrigger<SimpleTrigger>()
                .WithHandler<SimpleTriggerHandler>()
                .WithHandler<SimpleTriggerHandler2>();

            factory.AddQuery<SimpleQuery, bool>()
                .WithHandler<SimpleQueryHandler>();
        });
        
        await using IScopedProvider scopedProvider = collection.Build();
        var continuum = scopedProvider.GetRequiredService<IContinuum>();
        continuum.StartProcessing(); // This currently means that all hub within the message bus are always running

        bool doWhile = true;
        
        // ReSharper disable once LoopVariableIsNeverChangedInsideLoop
        while (doWhile) {
            Console.Write("Enter a string: ");
            if (Console.ReadLine() is not {} input) continue;

            await Parallel.ForAsync(0, 10, CancellationToken.None, async (i, ct) => {
                try {
                    switch (input.ToLowerInvariant()) {
                        case "trigger" or "t": {
                            await continuum.TriggerAsync(new SimpleTrigger($"{input};;{i}", DateTime.UtcNow), ct);
                            break;
                        }

                        case "command" or "c": {
                            bool result = await continuum.ExecuteAsync<SimpleCommand, bool>(new SimpleCommand($"{input};;{i}", DateTime.UtcNow), ct);
                            Console.WriteLine($"You entered: {input} and got : {result}");
                            break;
                        }

                        case "query" or "q" : {
                            bool queryResult = await continuum.QueryAsync<SimpleQuery, bool>(new SimpleQuery($"{input};;{i}", DateTime.UtcNow), ct);
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
    }
}
