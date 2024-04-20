﻿// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace AterraCore.DI;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public static class EngineServices {
    private static ServiceProvider ServiceProvider { get; set; } = null!;
    
    public static void BuildServiceProvider(IServiceCollection serviceCollection) {
        ServiceProvider = serviceCollection.BuildServiceProvider();
    }
    
    public static T GetService<T>() where T : notnull{
        try {
            return ServiceProvider.GetRequiredService<T>();
        }
        catch (InvalidOperationException e) {
            string? typeName = typeof(T).FullName;  // Get type name
            GetLogger().Fatal("Service type of {TypeOfT} could not be found.", typeName);
            throw new InvalidOperationException($"Service type of {typeName} could not be found.", e);
        }
    }

    public static T CreateWithServices<T>(Type objectType) {
        return (T)ActivatorUtilities.CreateInstance(ServiceProvider, objectType);
    }
    public static T CreateWithServices<T>(){
        return (T)ActivatorUtilities.CreateInstance(ServiceProvider, typeof(T));
    }
    
    // -----------------------------------------------------------------------------------------------------------------
    // Default Services Quick access
    // -----------------------------------------------------------------------------------------------------------------
    public static ILogger GetLogger() => GetService<ILogger>();
}