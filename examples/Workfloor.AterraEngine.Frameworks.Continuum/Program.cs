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
        collection.AddTransient(typeof(ICommandHub<,>), typeof(CommandHub<,>)); // TODO fix that CommandHub<,> can be registered as a service and not just an interface
        collection.AddTransientFromFactory<IMessageBus, IMessageBusFactory>();
        collection.AddSingletonFromFactory<IMessageBusFactory>(static provider => {
            var factory = new MessageBusFactory(provider);
            factory.AddCommand<SimpleCommand, bool>();
            return factory;
        });
        
        await using IScopedProvider scopedProvider = collection.Build();
        var messageBus = scopedProvider.GetRequiredService<IMessageBus>();
        messageBus.StartProcessingAsync();

        while (true) {
            Console.Write("Enter a string: ");
            if (Console.ReadLine() is not {} input) continue;
            _ = Task.Run(async () => {
                bool result = await messageBus.ExecuteAsync<SimpleCommand, bool>(new SimpleCommand(input, DateTime.UtcNow));
                Console.WriteLine($"You entered: {input} and got : {result}");
            });
        }
    }
}
