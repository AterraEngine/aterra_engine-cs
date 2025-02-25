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
        
        // Needed for Continuum to work 
        collection.AddContinuum(static factory => {
            factory.AddCommand<SimpleCommand, bool>()
                .WithHandler<SimpleCommandHandler>()
                .WithPipelineSteps(typeof(SimpleCommandPipelineStep<,>));
            // .WithPipelineStep<CommandPipelineStep<SimpleCommand, bool>>();

            factory.AddTrigger<SimpleTrigger>()
                .WithHandler<SimpleTriggerHandler>()
                .WithHandler<SimpleTriggerHandler2>();
        });

        collection.WithContinuum(static factory => {
            factory.AddQuery<SimpleQuery, bool>()
                .WithHandler<SimpleQueryHandler>();
        });

        await using IScopedProvider scopedProvider = collection.Build();
        var continuum = scopedProvider.GetRequiredService<IContinuum>();

        bool doWhile = true;
        CancellationToken ct = CancellationToken.None;
        
        // ReSharper disable once LoopVariableIsNeverChangedInsideLoop
        while (doWhile) {
            Console.Write("Enter a string: ");
            if (Console.ReadLine() is not {} input) continue;

            switch (input.ToLowerInvariant()) {
                case "trigger" or "t": {
                    await continuum.TriggerAsync(new SimpleTrigger($"{input}", DateTime.UtcNow), ct);
                    break;
                }

                case "command" or "c": {
                    bool result = await continuum.ExecuteAsync<SimpleCommand, bool>(new SimpleCommand($"{input}", DateTime.UtcNow), ct);
                    Console.WriteLine($"You entered: {input} and got : {result}");
                    break;
                }

                case "query" or "q": {
                    bool queryResult = await continuum.QueryAsync<SimpleQuery, bool>(new SimpleQuery($"{input}", DateTime.UtcNow), ct);
                    Console.WriteLine($"You entered: {input} and got : {queryResult}");

                    break;
                }

                case "cancel": {
                    try {
                        CancellationToken ctNew = new CancellationTokenSource( TimeSpan.FromMilliseconds(500) ).Token;
                        bool result = await continuum.ExecuteAsync<SimpleCommand, bool>(new SimpleCommand($"{input}", DateTime.UtcNow), ctNew);
                        Console.WriteLine($"You entered: {input} and got : {result}");
                    }
                    catch (Exception e) {
                        Console.WriteLine(e);
                    }
                    break;
                }

                case "exit" or "x": {
                    doWhile = false;
                    break;
                }
            }
        }

        Console.WriteLine("Exiting...");
    }
}
