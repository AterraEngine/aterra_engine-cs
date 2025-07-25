// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

using AterraEngine.Contracts;
using Microsoft.Extensions.DependencyInjection;

namespace AterraEngine.Shared;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public static class EngineExtensions {
    public static T GetRequiredService<T>(this IEngine engine) where T : notnull 
        => engine.ServiceProvider.GetRequiredService<T>();
    
    public static T? GetService<T>(this IEngine engine) where T : notnull 
        => engine.ServiceProvider.GetService<T>();
}