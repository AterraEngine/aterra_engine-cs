// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using Serilog;

namespace AterraCore.Loggers;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
/// <summary>
/// Contains extension methods for the <see cref="ILogger"/> interface.
/// </summary>
public static class SectionExtensions {
    /// <summary>
    /// Extends the logger with a context for AssetAtlas section.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    /// <returns>The logger instance with the AssetAtlas context added.</returns>
    public static ILogger ForAssetAtlasContext(this ILogger logger) => logger.ForContext("Section", "AssetAtlas");
    
    /// <summary>
    /// Returns a logger with the "Section" context set to "Engine".
    /// </summary>
    /// <param name="logger">The original logger.</param>
    /// <returns>A logger with the "Section" context set to "Engine".</returns>
    public static ILogger ForEngineContext(this ILogger logger) => logger.ForContext("Section", "Engine");
    
    /// <summary>
    /// Adds a context to the logger indicating that it is for the EngineServiceBuilder section.
    /// </summary>
    /// <param name="logger">The ILogger instance to add the context to.</param>
    /// <returns>The ILogger instance with the added context.</returns>
    public static ILogger ForEngineServiceBuilderContext(this ILogger logger) => logger.ForContext("Section", "EngineServiceBuilder");
    
    /// <summary>
    /// Retrieves a logger enriched with the "Section" context set to "EngineServices".
    /// </summary>
    /// <param name="logger">The logger to be enriched.</param>
    /// <returns>A logger with the "Section" context set to "EngineServices".</returns>
    public static ILogger ForEngineServicesContext(this ILogger logger) => logger.ForContext("Section", "EngineServices");
    
    /// <summary>
    /// Extends the ILogger with a context for the FlexiPlug section.
    /// </summary>
    /// <param name="logger">The ILogger instance to extend.</param>
    /// <returns>The ILogger instance with the FlexiPlug section context.</returns>
    public static ILogger ForFlexiPlugContext(this ILogger logger) => logger.ForContext("Section", "FlexiPlug");
    
    /// <summary>
    /// Creates a logger with context for the "Nexities" section.
    /// </summary>
    /// <param name="logger">The logger to be extended.</param>
    /// <returns>A logger with the context for the "Nexities" section.</returns>
    public static ILogger ForNexitiesContext(this ILogger logger) => logger.ForContext("Section", "Nexities");
    
    /// <summary>
    /// Retrieves a logger with the "Raylib" section context.
    /// </summary>
    /// <param name="logger">The logger to extend.</param>
    /// <returns>A logger with the "Raylib" section context.</returns>
    public static ILogger ForRaylibContext(this ILogger logger) => logger.ForContext("Section", "Raylib");
    
    /// <summary>
    /// Returns a new logger that is enriched with the 'Section' context set to "Startup".
    /// </summary>
    /// <param name="logger">The original logger to be enriched.</param>
    /// <returns>A new logger with the 'Section' context set to "Startup".</returns>
    public static ILogger ForStartupContext(this ILogger logger) => logger.ForContext("Section", "Startup");
    
    /// <summary>
    /// Retrieves a logger context for the TextureAtlas section.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    /// <returns>The logger instance with the TextureAtlas section context.</returns>
    public static ILogger ForTextureAtlasContext(this ILogger logger) => logger.ForContext("Section", "TextureAtlas");
    
    /// <summary>
    /// Creates a new logger with the context "ConfigurationWarningAtlas".
    /// </summary>
    /// <param name="logger">The original ILogger instance.</param>
    /// <returns>A new ILogger instance with the context "ConfigurationWarningAtlas".</returns>
    public static ILogger ForConfigurationWarningAtlasContext(this ILogger logger) => logger.ForContext("Section", "ConfigurationWarningAtlas");
}
