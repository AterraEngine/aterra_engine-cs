// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using Serilog;
using Serilog.Core;

namespace AterraCore.Loggers;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
/// <summary>
///     Contains extension methods for the <see cref="ILogger" /> interface.
/// </summary>
public static class SectionExtensions {
    public static ILogger ForContext<T>(this ILogger logger) => logger.ForContext(Constants.SourceContextPropertyName, typeof(T).Namespace);

    public static ILogger ForBootOperationContext(this ILogger logger, Type section) => logger.ForContext("IsBootOperation", "BO:").ForContext(Constants.SourceContextPropertyName, section.Name);
    public static ILogger ForBootOperationContext<T>(this ILogger logger) => logger.ForContext("IsBootOperation", "BO:").ForContext(Constants.SourceContextPropertyName, typeof(T).Name);
    public static ILogger ForBootOperationContext(this ILogger logger, string section) => logger.ForContext("IsBootOperation", "BO:").ForContext(Constants.SourceContextPropertyName, section);

    public static ILogger ForPluginLoaderContext(this ILogger logger, Type section) => logger.ForContext("IsBootOperation", "PL:").ForContext(Constants.SourceContextPropertyName, section.Name);
    public static ILogger ForPluginLoaderContext<T>(this ILogger logger) => logger.ForContext("IsBootOperation", "PL:").ForContext(Constants.SourceContextPropertyName, typeof(T).Name);
    public static ILogger ForPluginLoaderContext(this ILogger logger, string section) => logger.ForContext("IsBootOperation", "PL:").ForContext(Constants.SourceContextPropertyName, section);
}
