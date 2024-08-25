// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Common.Data;
using AterraCore.Loggers;
using Serilog;
using Serilog.Core;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace AterraEngine.Threading.Render;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public static class RaylibLogger {
    // ReSharper disable once InconsistentNaming
    private static ILogger _logger { get; } = CreateConfiguration().CreateLogger().ForContext(Constants.SourceContextPropertyName, "RayLib");

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    private static LoggerConfiguration CreateConfiguration() =>
        new LoggerConfiguration()
            .MinimumLevel.Verbose()
            .AsyncSinkFile(Paths.Logs.RendererLog)
            .SinkConsole();// Allow async console as well?

    // .AsyncSinkConsole();
    // Extra Raylib stuff here
    public static unsafe delegate* unmanaged[Cdecl]<int, sbyte*, sbyte*, void> GetPointer() => &LogCallback;

    [UnmanagedCallersOnly(CallConvs = [typeof(CallConvCdecl)])]
    public static unsafe void LogCallback(int msgType, sbyte* text, sbyte* args) {
        // This is Raylib_cs.Logging
        string message = Logging.GetLogMessage(new IntPtr(text), new IntPtr(args));

        Action<string> callback = (TraceLogLevel)msgType switch {
            // Comments retrieved from Raylib-cs:Examples.Core.CustomLogging.cs
            TraceLogLevel.All => _logger.Information,// Display all logs
            TraceLogLevel.Trace => _logger.Verbose,// Trace logging, intended for internal use only
            TraceLogLevel.Debug => _logger.Debug,// Debug logging, used for internal debugging, it should be disabled on release builds
            TraceLogLevel.Info => _logger.Information,// Info logging, used for program execution info
            TraceLogLevel.Warning => _logger.Warning,// Warning logging, used on recoverable failures
            TraceLogLevel.Error => _logger.Error,// Error logging, used on unrecoverable failures
            TraceLogLevel.Fatal => _logger.Fatal,// Fatal logging, used to abort program: exit(EXIT_FAILURE)
            TraceLogLevel.None => _ => {},// Disable logging
            _ => msg => _logger.Error("Could not be mapped to a TraceLogLevel: {msgType} - {msg}", msgType, msg)
        };

        callback(message);
    }
}
