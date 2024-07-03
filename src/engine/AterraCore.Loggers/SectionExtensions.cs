// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using Serilog;

namespace AterraCore.Loggers;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------

public static class SectionExtensions {
    public static ILogger ForStartupContext(this ILogger logger) {
        return logger.ForContext("Section", "Startup");
    }
    
    public static ILogger ForRaylibContext(this ILogger logger) {
        return logger.ForContext("Section", "Raylib");
    }
    
    public static ILogger ForNexitiesContext(this ILogger logger) {
        return logger.ForContext("Section", "Nexities");
    }
    
    public static ILogger ForFlexiPlugContext(this ILogger logger) {
        return logger.ForContext("Section", "FlexiPlug");
    }
    
    public static ILogger ForAssetAtlasContext(this ILogger logger) {
        return logger.ForContext("Section", "AssetAtlas");
    }
    
    public static ILogger ForEngineContext(this ILogger logger) {
        return logger.ForContext("Section", "AssetAtlas");
    }
}
