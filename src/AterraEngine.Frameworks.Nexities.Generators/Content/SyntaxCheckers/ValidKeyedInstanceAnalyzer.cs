// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace AterraEngine.Frameworks.Nexities.Generators.Content.SyntaxCheckers;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------

[DiagnosticAnalyzer(LanguageNames.CSharp)]
public class ValidKeyedInstanceAnalyzer : DiagnosticAnalyzer{
    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get; } = ImmutableArray.Create<DiagnosticDescriptor>([
        Diagnostics.InvalidGuidDescriptor
    ]);
    
    public override void Initialize(AnalysisContext context) {
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.Analyze | GeneratedCodeAnalysisFlags.ReportDiagnostics);
        context.EnableConcurrentExecution();
        
        context.RegisterSymbolAction(Action, SymbolKind.Property);
    }
    
    private void Action(SymbolAnalysisContext context) {
        if (context.Symbol is not IPropertySymbol property) return;
        IEnumerable<AttributeData> attributes = property
            .GetAttributes()
            .Where(attribute => attribute.AttributeClass?.ToDisplayString() == SymbolNames.OmniaIdAttribute_DisplayString);

        foreach (AttributeData attribute in attributes) {
            if (attribute.ConstructorArguments.Length == 0) continue;
            if (attribute.ConstructorArguments[0].Value is not string value) continue;

            if (!Guid.TryParse(value, out _)) {
                context.ReportDiagnostic(Diagnostic.Create(Diagnostics.InvalidGuidDescriptor, property.Locations[0], property.Name));
            }
        }
    }
}