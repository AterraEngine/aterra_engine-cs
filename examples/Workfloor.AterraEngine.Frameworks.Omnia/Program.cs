// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.DependencyInjection;
using AterraEngine.Frameworks.Omnia;
using AterraEngine.Frameworks.Omnia.PreProcessor;
using Workfloor.AterraEngine.Frameworks.Omnia.Assets;

namespace Workfloor.AterraEngine.Frameworks.Omnia;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public static class Program {
    public static async Task Main(string[] args) {
        // Plugin Init
        IScopedProvider pluginProvider = await PluginInit(); 
        
        // Post Plugin Initialization
        IScopedProvider engineProvider = await PostPluginInit(pluginProvider);

        var registrationLibrary = engineProvider.GetRequiredService<IOmniaRegistrationLibrary>();
        bool result = registrationLibrary.TryGetRegistration("workfloor:assets/simple", out IOmniaRegistration? registration);
        
        Console.WriteLine($"Result: {result}");
        Console.WriteLine($"Registration: {registration}");
        
        Console.WriteLine();
        Console.WriteLine();
        Console.WriteLine();
        Console.WriteLine();
        
        bool resultByType = registrationLibrary.TryGetRegistration(typeof(SimpleAsset), out IOmniaRegistration? registrationB);
        Console.WriteLine($"Result: {resultByType}");
        Console.WriteLine($"Registration: {registrationB}");
        
        Console.WriteLine();
        Console.WriteLine();
        Console.WriteLine();
        Console.WriteLine();
        
        var library = engineProvider.GetRequiredService<IOmniaAssetLibrary>();
        bool instanceResult = library.TryGetInstance("workfloor:assets/simple", out SimpleAsset? instance);
        Console.WriteLine($"Result: {instanceResult}");
        Console.WriteLine($"Instance: {instance?.InstanceId} - {instance?.OmniaId}");
        Console.WriteLine($"name: {instance?.Name}");
        
        Console.WriteLine();
        Console.WriteLine("Cleaning up");
        library.ReturnInstance(instance!);
        
        
        Console.WriteLine($"Instance: {instance?.InstanceId} - {instance?.OmniaId}");
        Console.WriteLine($"name: {instance?.Name}");
        
        
        
        
        // var engine = engineProvider.GetRequiredService<IAterraEngine>();
        // await engine.RunAsync();
    }
    
    private static async Task<IScopedProvider> PluginInit() {
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddSingleton<IOmniaRegistrationCollector, OmniaRegistrationCollector>();
        
        IScopedProvider provider = serviceCollection.Build();
        
        // Actual stuff the pre-processor should do
        var collector = provider.GetRequiredService<IOmniaRegistrationCollector>();
        
        // These sort of registrations should be automagically assigned by a generator? Appended to with data from a .json file?
        collector.AddRegistration(new OmniaRegistration<SimpleAsset>("workfloor:assets/simple") {
            MaxPoolSize = 1000,
            PoolPolicyBuilder = static provider => provider.GetRequiredService<OmniaAssetPoolPolicy<SimpleAsset>>()
        });
        
        return provider;
    }

    private static async Task<IScopedProvider> PostPluginInit(IScopedProvider pluginProvider) {
        var serviceCollection = new ServiceCollection();
        
        serviceCollection.AddSingleton<IOmniaRegistrationCollector>(pluginProvider.GetRequiredService<IOmniaRegistrationCollector>());
        serviceCollection.AddTransient(typeof(OmniaAssetPoolPolicy<>));
        
        serviceCollection.AddTransient<SimpleAsset>();
        
        serviceCollection.AddSingleton<IOmniaRegistrationComparer, OmniaRegistrationComparer>();
        serviceCollection.AddSingleton<IOmniaRegistrationLibraryFactory, OmniaRegistrationLibraryFactory>();
        serviceCollection.AddSingletonFromFactory<IOmniaRegistrationLibrary, IOmniaRegistrationLibraryFactory>();
        serviceCollection.AddSingleton<IOmniaAssetLibrary, OmniaAssetLibrary>();
        
        IScopedProvider provider = serviceCollection.Build();
        return provider;
    }
}
