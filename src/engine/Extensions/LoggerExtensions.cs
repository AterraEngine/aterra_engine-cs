// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

using AterraCore.Common;
using Serilog;
using Serilog.Core;

namespace Extensions;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------

public static class LoggerExtensions {
    [MessageTemplateFormatMethod("messageTemplate")]
    public static void ThrowFatal(this ILogger logger, string messageTemplate, params object?[]? propertyValues) {
        ThrowFatal<Exception>(logger, messageTemplate, propertyValues);
    }
    
    [MessageTemplateFormatMethod("messageTemplate")]
    public static void ThrowFatal<TException>(this ILogger logger, string messageTemplate, params object?[]? propertyValues) where TException : Exception, new() {
        var exception = (TException)Activator.CreateInstance(typeof(TException), messageTemplate)!;
        logger.Fatal(exception, messageTemplate, propertyValues);
        throw exception;
    }
    
    [MessageTemplateFormatMethod("messageTemplate")]
    public static void ExitFatal(this ILogger logger, ExitCodes exitCode, string messageTemplate, params object?[]? propertyValues) {
        logger.Fatal(messageTemplate, propertyValues);
        Environment.Exit((int)exitCode);
    }
}