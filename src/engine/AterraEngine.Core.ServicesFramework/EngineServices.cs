// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using Microsoft.Extensions.DependencyInjection;

namespace AterraEngine.Core.ServicesFramework;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public static class EngineServices {
    private static ServiceProvider ServiceProvider { get; set; } = null!;
    
    public static void BuildServiceProvider(IServiceCollection serviceCollection) {
        ServiceProvider = serviceCollection.BuildServiceProvider();
        EngineStartupLogger.Log.Information("Service provider built");
    }
    public static void DisposeServiceProvider() {
        ServiceProvider.Dispose();
    }
    
    public static T GetService<T>() where T : notnull{
        return ServiceProvider.GetRequiredService<T>();
    }

    public static T CreateWithServices<T>(Type objectType) {
        return (T)ActivatorUtilities.CreateInstance(ServiceProvider, objectType);
    }
    public static T CreateWithServices<T>(){
        return (T)ActivatorUtilities.CreateInstance(ServiceProvider, typeof(T));
    }
    
}