// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.DependencyInjection;
using AterraEngine.DependencyInjection.Bridges.Microsoft;
using AterraEngine.Frameworks.Omnia;
using AterraEngine.Frameworks.Omnia.PreProcessor;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using Workfloor.AterraEngine.Frameworks.Omnia.Assets;
using ILogger=Microsoft.Extensions.Logging.ILogger;
using ServiceCollection=AterraEngine.DependencyInjection.ServiceCollection;

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
        ILogger logger = engineProvider.GetRequiredService<ILoggerFactory>().CreateLogger("Main");

        var registrationLibrary = engineProvider.GetRequiredService<IOmniaRegistrationLibrary>();
        bool result = registrationLibrary.TryGetRegistration("workfloor:assets/simple", out IOmniaRegistration? registration);
        
        logger.Information("Result: {result}", result);
        logger.Information("Registration: {registration}", registration);
        
        bool resultByType = registrationLibrary.TryGetRegistration(typeof(SimpleAsset), out IOmniaRegistration? registrationB);
    
        logger.Information("Result 2: {result}", resultByType);
        logger.Information("Registration 2: {registration}", registrationB);
        
        var library = engineProvider.GetRequiredService<IOmniaAssetLibrary>();
        bool instanceResult = library.TryGetInstance("workfloor:assets/simple", out SimpleAsset? instance);
        logger.Information("Instance Result: {result}", instanceResult);
        logger.Information("Instance: {instance}", instance);
        logger.Information("Instance OmniaId: {omniaId}", instance?.OmniaId);
        logger.Information("Instance Name: {name}", instance?.Name);
        
        logger.Information("Cleaning up...");
        library.ReturnInstance(instance!);

        logger.Information("Cleaned up!");
        logger.Information("Instance Result: {result}", instanceResult);
        logger.Information("Instance: {instance}", instance);
        logger.Information("Instance OmniaId: {omniaId}", instance?.OmniaId);
        logger.Information("Instance Name: {name}", instance?.Name);
        
        await Log.CloseAndFlushAsync();
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
        var serviceCollection = new MsBridgeServiceCollection();
        
        serviceCollection.AddSingleton<IOmniaRegistrationCollector>(pluginProvider.GetRequiredService<IOmniaRegistrationCollector>());
        serviceCollection.AddTransient(typeof(OmniaAssetPoolPolicy<>));
        
        serviceCollection.AddTransient<SimpleAsset>();
        
        serviceCollection.AddSingleton<IOmniaRegistrationComparer, OmniaRegistrationComparer>();
        serviceCollection.AddSingleton<IOmniaRegistrationLibraryFactory, OmniaRegistrationLibraryFactory>();
        serviceCollection.AddSingletonFromFactory<IOmniaRegistrationLibrary, IOmniaRegistrationLibraryFactory>();
        serviceCollection.AddSingleton<IOmniaAssetLibrary, OmniaAssetLibrary>();
        
        Log.Logger = new LoggerConfiguration()
            .AsAnnaSasDevServerConsole(
                24,
                configure: asyncConsoleConfig => asyncConsoleConfig.ApplyThemeToRedirectedOutput = true)
            .Enrich.FromLogContext()
            .CreateLogger();

        serviceCollection.MsServiceCollection.AddLogging(builder => builder.AddSerilog());
        
        IScopedProvider provider = serviceCollection.Build();
        return provider;
    }
}
