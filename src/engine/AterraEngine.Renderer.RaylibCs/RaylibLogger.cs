// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using AterraCore.Common;
using AterraCore.Loggers;
using Serilog;


namespace AterraEngine.Renderer.RaylibCs;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class RaylibLogger {
    // ReSharper disable once InconsistentNaming
    private static ILogger _logger { get; } = CreateConfiguration().CreateLogger();

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public static LoggerConfiguration CreateConfiguration() {
        return new LoggerConfiguration()
            .MinimumLevel.Verbose()

            .DefaultEnrich("Raylib")
            .AsyncSinkFile(Paths.Logs.RendererLog)
            .SinkConsole();
             // .AsyncSinkConsole();
    }
    
    // Extra Raylib stuff here
    public unsafe delegate* unmanaged[Cdecl]<int, sbyte*, sbyte*, void> GetPointer() => &LogCallback;
    
    [UnmanagedCallersOnly(CallConvs = [typeof(CallConvCdecl)])]
    public static unsafe void LogCallback(int msgType, sbyte* text, sbyte* args) {
        // This is Raylib_cs.Logging
        var message = Logging.GetLogMessage(new IntPtr(text), new IntPtr(args));
        _logger.Information(message);
    }
}