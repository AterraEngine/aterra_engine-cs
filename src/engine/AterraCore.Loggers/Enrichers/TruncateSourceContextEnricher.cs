// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using Serilog.Core;
using Serilog.Events;

namespace AterraCore.Loggers.Enrichers;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class TruncateSourceContextEnricher(int maxLength) : ILogEventEnricher {
    public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory) {
        if (!logEvent.Properties.TryGetValue("SourceContext", out LogEventPropertyValue? sourceContextValue)
            || sourceContextValue is not ScalarValue { Value: string sourceContext }) return;

        string truncatedSourceContext = sourceContext.Length > maxLength + 3
            ? string.Concat("...", sourceContext.AsSpan(sourceContext.Length - maxLength, maxLength))
            : sourceContext;

        var truncatedProperty = new LogEventProperty("SourceContext", new ScalarValue(truncatedSourceContext));
        logEvent.AddOrUpdateProperty(truncatedProperty);
    }
}
