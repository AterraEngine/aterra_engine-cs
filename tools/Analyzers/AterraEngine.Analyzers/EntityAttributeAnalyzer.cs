// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
namespace AterraEngine.Analyzers;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------

[DiagnosticAnalyzer(LanguageNames.CSharp)]
public class EntityAttributeAnalyzer : DiagnosticAnalyzer {
    public const string DiagnosticId = "AE0001";
    private const string Category = "Naming";

    private static readonly DiagnosticDescriptor Rule = new(
        DiagnosticId,
        new LocalizableResourceString(nameof(Resources.AE0001Title), Resources.ResourceManager, typeof(Resources)),
        new LocalizableResourceString(nameof(Resources.AE0001MessageFormat), Resources.ResourceManager, typeof(Resources)),
        Category,
        DiagnosticSeverity.Error,
        true,
        new LocalizableResourceString(nameof(Resources.AE0001Description), Resources.ResourceManager, typeof(Resources))
    );

    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get; } = [Rule];


    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public override void Initialize(AnalysisContext context) {
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
        context.EnableConcurrentExecution();
        context.RegisterSymbolAction(AnalyzeSymbol, SymbolKind.NamedType);
    }

    private void AnalyzeSymbol(SymbolAnalysisContext context) {
        ISymbol symbol = context.Symbol;

        // Use the full name of the attribute as a string
        AttributeData? customAttribute = symbol
            .GetAttributes()
            .FirstOrDefault(ad => ad.AttributeClass?.ToDisplayString() == "AterraCore.Nexities.Entities.EntityAttribute");
        if (customAttribute is null) return;

        // Continue with the name of the argument
        TypedConstant instanceTypeArg = customAttribute.NamedArguments.Any(na => na.Key == "instanceType")
                                        && customAttribute.NamedArguments.Single(na => na.Key == "instanceType").Value.Value is not null
            ? customAttribute.NamedArguments.Single(na => na.Key == "instanceType").Value
            : customAttribute.ConstructorArguments.Skip(1).DefaultIfEmpty(new TypedConstant()).First();

        // 3 is the value of AssetInstanceType.Pooled
        if (instanceTypeArg.Value is not 3) return;

        context.ReportDiagnostic(
            Diagnostic.Create(
                Rule,
                customAttribute.ApplicationSyntaxReference?.GetSyntax().GetLocation(),
                instanceTypeArg.Value
            ));
    }
}