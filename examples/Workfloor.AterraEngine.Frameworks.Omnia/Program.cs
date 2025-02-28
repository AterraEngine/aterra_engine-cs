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
        
        // var engine = engineProvider.GetRequiredService<IAterraEngine>();
        // await engine.RunAsync();
    }
    
    private static async Task<IScopedProvider> PluginInit() {
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddSingleton<IOmniaRegistrationCollector, OmniaRegistrationCollector>();
        serviceCollection.AddTransient(typeof(OmniaAssetPoolPolicy<>));
        IScopedProvider provider = serviceCollection.Build();
        
        // Actual stuff the pre-processor should do
        var collector = provider.GetRequiredService<IOmniaRegistrationCollector>();
        
        // These sort of registrations should be automagically assigned by a generator? Appended to with data from a .json file?
        collector.AddRegistration(new OmniaRegistration<SimpleAsset>("workfloor:assets/simple") {
            MaxPoolSize = 1000,
            PoolPolicy = provider.GetRequiredService<OmniaAssetPoolPolicy<SimpleAsset>>(),
        });
        
        return provider;
    }

    private static async Task<IScopedProvider> PostPluginInit(IScopedProvider pluginProvider) {
        var serviceCollection = new ServiceCollection();
                
        serviceCollection.AddSingletonFromFactory<IOmniaRegistrationLibrary>(_ => {
            var collector = pluginProvider.GetRequiredService<IOmniaRegistrationCollector>();
            
            var factory = new OmniaRegistrationLibraryFactory();
            foreach (IOmniaRegistration registration in collector.GetRegistrations()) {
                factory.AddRegistration(registration);
            }
            
            IOmniaRegistrationLibraryFactory frozen = factory.ToFrozen();
            return frozen.Create(pluginProvider);
        });
        
        IScopedProvider provider = serviceCollection.Build();
        return provider;
    }
}
