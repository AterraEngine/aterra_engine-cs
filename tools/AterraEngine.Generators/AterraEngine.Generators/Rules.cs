// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using Microsoft.CodeAnalysis;

namespace AterraEngine.Generators;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public static class Rules {
    public static readonly DiagnosticDescriptor NoAttributesFound = new(
        "ATRGEN001",
        "InjectableServiceAttribute not found",
        "InjectableServiceAttribute not found",
        "SourceGenerator",
        DiagnosticSeverity.Info,
        true
    );
    
    public static readonly DiagnosticDescriptor NoRegistrationsGenerated = new(
        "ATRGEN002",
        "No registrations were generated",
        "No registrations were generated",
        "SourceGenerator",
        DiagnosticSeverity.Warning,
        true
    );
}
