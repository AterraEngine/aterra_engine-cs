// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Loggers.Enrichers;
using Serilog;
using Serilog.Formatting.Compact;
using Serilog.Sinks.SystemConsole.Themes;
using static CodeOfChaos.Ansi.AnsiColor;

namespace AterraCore.Loggers;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
/// <summary>
///     Extensions for configuring the Serilog LoggerConfiguration.
/// </summary>
public static class LoggerConfigurationExtensions {
    /// <summary>
    ///     The output template used for formatting log messages.
    /// </summary>
    public const string OutputTemplateEngine = "[ {SourceContext,24} : {Timestamp:HH:mm:ss.fff} : {Level:u3}] | {Message:lj} {NewLine}{Exception}";

    public const string OutputTemplateStartup = "[ {IsBootOperation,-4}{SourceContext,20} : {Timestamp:HH:mm:ss.fff} : {Level:u3}] | {Message:lj} {NewLine}{Exception}";

    #region Theme
    /// <summary>
    ///     Represents the theme configuration for the Serilog logger console sink.
    /// </summary>
    private static readonly AnsiConsoleTheme Theme = new(
        new Dictionary<ConsoleThemeStyle, string> {
            [ConsoleThemeStyle.Text] = AsFore("white"),
            [ConsoleThemeStyle.SecondaryText] = AsFore("silver"),
            [ConsoleThemeStyle.TertiaryText] = AsFore("gray"),

            [ConsoleThemeStyle.Invalid] = AsFore("gold"),
            [ConsoleThemeStyle.Null] = AsFore("coral"),
            [ConsoleThemeStyle.Name] = AsFore("slategray"),
            [ConsoleThemeStyle.String] = AsFore("aqua"),
            [ConsoleThemeStyle.Number] = AsFore("mediumpurple"),
            [ConsoleThemeStyle.Boolean] = AsFore("coral"),
            [ConsoleThemeStyle.Scalar] = AsFore("coral"),

            [ConsoleThemeStyle.LevelVerbose] = AsFore("silver"),
            [ConsoleThemeStyle.LevelDebug] = AsFore("rose"),
            [ConsoleThemeStyle.LevelInformation] = AsFore("white"),
            [ConsoleThemeStyle.LevelWarning] = AsFore("gold"),
            [ConsoleThemeStyle.LevelError] = AsFore("white") + AsBack("rose"),
            [ConsoleThemeStyle.LevelFatal] = AsFore("white") + AsBack("maroon")
        });
    #endregion

    // -----------------------------------------------------------------------------------------------------------------
    // Extensions
    // -----------------------------------------------------------------------------------------------------------------
    /// <summary>
    ///     Adds default enrichments to the LoggerConfiguration.
    /// </summary>
    /// <param name="lc">The LoggerConfiguration object.</param>
    /// <param name="stage">The stage of the application.</param>
    /// <returns>A LoggerConfiguration object with default enrichments added.</returns>
    public static LoggerConfiguration DefaultEnrich(this LoggerConfiguration lc, string stage) =>
        lc
            .Enrich.FromLogContext()
            .Enrich.WithProperty("Application", "AterraEngineOLD")
            .Enrich.WithProperty("Stage", stage)
            .Enrich.WithProperty("MachineName", Environment.MachineName)
            .Enrich.WithThreadId()
            .Enrich.WithMemoryUsage()
            .Enrich.With(new TruncateSourceContextEnricher(17));

    /// <summary>
    ///     Adds an async file sink to the logger configuration for writing logs asynchronously.
    /// </summary>
    /// <param name="lc">The logger configuration.</param>
    /// <param name="filePath">The path to the log file.</param>
    /// <returns>The updated logger configuration.</returns>
    public static LoggerConfiguration AsyncSinkFile(this LoggerConfiguration lc, string filePath) {
        return lc
            // Using Async Sink to write logs asynchronously 
            // to avoid any performance issues during gameplay
            .WriteTo.Async(lsc => lsc.File(
                new CompactJsonFormatter(),
                filePath,
                rollingInterval: RollingInterval.Day
            ));
    }

    /// <summary>
    ///     Writes log events to the console sink with default configuration.
    /// </summary>
    /// <param name="lc">The <see cref="LoggerConfiguration" /> object.</param>
    /// <param name="outputTemplate">Serilog console template</param>
    /// <returns>The updated <see cref="LoggerConfiguration" /> object.</returns>
    public static LoggerConfiguration DefaultSinkConsole(this LoggerConfiguration lc, string? outputTemplate = null) =>
        lc.WriteTo.Console(
            theme: Theme,
            outputTemplate: outputTemplate ?? OutputTemplateEngine
        );

    /// <summary>
    ///     Configures Serilog logger to write log events to the console synchronously or asynchronously.
    ///     Using the async sink avoids any performance issues during gameplay.
    /// </summary>
    /// <param name="lc">The LoggerConfiguration object.</param>
    /// <param name="outputTemplate">Serilog console template</param>
    /// <returns>The updated LoggerConfiguration object.</returns>
    public static LoggerConfiguration AsyncSinkConsole(this LoggerConfiguration lc, string? outputTemplate = null) {
        return lc
            // Using Async Sink to write logs asynchronously 
            // to avoid any performance issues during gameplay
            .WriteTo.Async(lsc => lsc.Console(
                theme: Theme,
                outputTemplate: outputTemplate ?? OutputTemplateEngine
            ));
    }

    /// <summary>
    ///     Sets up the console sink for Serilog logging.
    /// </summary>
    /// <param name="lc">The <see cref="LoggerConfiguration" /> instance.</param>
    /// <param name="outputTemplate">Serilog console template</param>
    /// <returns>The <see cref="LoggerConfiguration" /> instance with console sink configured.</returns>
    public static LoggerConfiguration SinkConsole(this LoggerConfiguration lc, string? outputTemplate = null) =>
        lc
            // Using Async Sink to write logs asynchronously 
            // to avoid any performance issues during gameplay
            .WriteTo.Console(
                theme: Theme,
                outputTemplate: outputTemplate ?? OutputTemplateEngine
            );

    /// <summary>
    ///     Sets the console sink for the logger configuration.
    /// </summary>
    /// <param name="lc">The logger configuration.</param>
    /// <param name="allowAsync">A boolean flag indicating whether to use asynchronous console sink or not.</param>
    /// <param name="outputTemplate"></param>
    /// <returns>The updated logger configuration.</returns>
    public static LoggerConfiguration SetConsole(this LoggerConfiguration lc, bool allowAsync, string? outputTemplate = null) => allowAsync ? lc.AsyncSinkConsole(outputTemplate) : lc.SinkConsole(outputTemplate);
}
