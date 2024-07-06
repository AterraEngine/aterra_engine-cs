// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using Serilog;
using Serilog.Formatting.Compact;
using Serilog.Sinks.SystemConsole.Themes;
using static CodeOfChaos.Ansi.AnsiColor;

namespace AterraCore.Loggers;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public static class LoggerConfigurationExtensions {
    private const string OutputTemplate = "[{Section,24} : {Timestamp:HH:mm:ss.fff} : {Level:u3}] | {Message:lj}{NewLine}{Exception}";

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

    // -----------------------------------------------------------------------------------------------------------------
    // Extensions
    // -----------------------------------------------------------------------------------------------------------------
    public static LoggerConfiguration DefaultEnrich(this LoggerConfiguration lc, string stage) =>
        lc
            .Enrich.FromLogContext()
            .Enrich.WithProperty("Application", "AterraEngine")
            .Enrich.WithProperty("Stage", stage)
            .Enrich.WithProperty("MachineName", Environment.MachineName)
            .Enrich.WithThreadId()
            .Enrich.WithMemoryUsage();

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

    public static LoggerConfiguration DefaultSinkConsole(this LoggerConfiguration lc) =>
        lc.WriteTo.Console(
        theme: Theme,
        outputTemplate: OutputTemplate
        );

    public static LoggerConfiguration AsyncSinkConsole(this LoggerConfiguration lc) {
        return lc
            // Using Async Sink to write logs asynchronously 
            // to avoid any performance issues during gameplay
            .WriteTo.Async(lsc => lsc.Console(
            theme: Theme,
            outputTemplate: OutputTemplate
            ));
    }

    public static LoggerConfiguration SinkConsole(this LoggerConfiguration lc) =>
        lc
            // Using Async Sink to write logs asynchronously 
            // to avoid any performance issues during gameplay
            .WriteTo.Console(
            theme: Theme,
            outputTemplate: OutputTemplate
            );

    public static LoggerConfiguration SetConsole(this LoggerConfiguration lc, bool allowAsync) => allowAsync ? lc.AsyncSinkConsole() : lc.SinkConsole();
}
