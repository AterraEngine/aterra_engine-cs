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
        "InjectableServices",
        DiagnosticSeverity.Info,
        true
    );

    public static readonly DiagnosticDescriptor NoServicesRegistered = new(
        "ATRGEN002",
        "No Service registrations were generated",
        "No Service registrations were generated",
        "InjectableServices",
        DiagnosticSeverity.Warning,
        true
    );

    public static readonly DiagnosticDescriptor NoAssemblyNameFound = new(
        "ATRGEN003",
        "No assembly name was found",
        "No assembly name was found",
        "SourceGenerator",
        DiagnosticSeverity.Error,
        true
    );
}
