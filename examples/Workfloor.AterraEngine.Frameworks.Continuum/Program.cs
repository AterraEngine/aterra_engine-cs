// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.DependencyInjection;
using AterraEngine.Frameworks.Continuum;
using Workfloor.AterraEngine.Frameworks.Continuum.CommandHandlers;

namespace Workfloor.AterraEngine.Frameworks.Continuum;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public static class Program {
    public static async Task Main() {
        var collection = new ServiceCollection();
        collection.AddTransient<SimpleCommandHandler>();
        collection.AddTransientFromFactory<IMessageBus, MessageBusFactory>();
        collection.AddTransientFromFactory<MessageBusFactory>(provider => {
            var factory = new MessageBusFactory();
            
            var simpleCommandHandler = provider.GetRequiredService<SimpleCommandHandler>();
            CommandHub<SimpleCommand, bool> hub = CommandHub<SimpleCommand, bool>.FromHandler(simpleCommandHandler);
            factory.RegisteredCommandHubs.TryAdd(typeof(SimpleCommand),hub );
            return factory;
        });
        
        await using IScopedProvider scopedProvider = collection.Build();
        var messageBus = scopedProvider.GetRequiredService<IMessageBus>();
        messageBus.StartProcessingAsync();

        while (true) {
            Console.Write("Enter a string: ");
            if (Console.ReadLine() is not {} input) continue;
            bool result = await messageBus.ExecuteAsync<SimpleCommand, bool>(new SimpleCommand(input));
            Console.WriteLine($"You entered: {input} and got : {result}");
        }
    }
}
