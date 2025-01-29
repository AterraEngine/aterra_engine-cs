// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using Microsoft.CodeAnalysis;

namespace AterraEngine.Frameworks.Nexities.Generators;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public static class Diagnostics {
    public static readonly DiagnosticDescriptor InvalidGuidDescriptor = new(
        id: "NX0001",
        title: "Invalid GUID",
        messageFormat: "'{0}' is not a valid GUID",
        category: "Syntax",
        DiagnosticSeverity.Error,
        isEnabledByDefault: true);

}
