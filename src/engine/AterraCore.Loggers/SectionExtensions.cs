// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using Serilog;

namespace AterraCore.Loggers;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------

public static class SectionExtensions {
    public static ILogger ForStartupContext(this ILogger logger) => logger.ForContext("Section", "Startup");
    public static ILogger ForRaylibContext(this ILogger logger) => logger.ForContext("Section", "Raylib");
    public static ILogger ForNexitiesContext(this ILogger logger) => logger.ForContext("Section", "Nexities");
    public static ILogger ForFlexiPlugContext(this ILogger logger) => logger.ForContext("Section", "FlexiPlug");
    public static ILogger ForAssetAtlasContext(this ILogger logger) => logger.ForContext("Section", "AssetAtlas");
    public static ILogger ForEngineContext(this ILogger logger) => logger.ForContext("Section", "Engine");
    public static ILogger ForTextureAtlasContext(this ILogger logger) => logger.ForContext("Section", "TextureAtlas");
}
