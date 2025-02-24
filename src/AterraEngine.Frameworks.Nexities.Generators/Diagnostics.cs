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
        "NX0001",
        "Invalid GUID",
        "'{0}' is not a valid GUID",
        "Syntax",
        DiagnosticSeverity.Error,
        true);
}
