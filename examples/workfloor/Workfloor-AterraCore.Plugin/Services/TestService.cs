// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

using AterraCore.FlexiPlug.Attributes;
using Serilog;
using Serilog.Events;

namespace Workfloor_AterraCore.Plugin.Services;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[Service(typeof(ILogger))]
public class TestService : ILogger {
    public void Write(LogEvent logEvent) {
        if (logEvent == null) 
            throw new ArgumentNullException(nameof(logEvent));
        // Outputs the log message
        Console.WriteLine(logEvent.MessageTemplate.Text);
        // Outputs the properties
        foreach(KeyValuePair<string, LogEventPropertyValue> property in logEvent.Properties) {
            Console.WriteLine($"Key: {property.Key}, Value: {property.Value}");
        }
    }
}