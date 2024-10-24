// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading;

namespace AterraEngine.Generators;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[Generator(LanguageNames.CSharp)]
public class InjectableServicesGenerator : IIncrementalGenerator {

    public const string GeneratedFileName = "InjectableServicesExtensions.g.cs";
    public const string AttributeMetadataName = "AterraEngine.Common.Attributes.InjectableServiceAttribute`1";
    private static ImmutableArray<string> _validLifeTimes = ["Singleton", "Scoped", "Transient"];

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public void Initialize(IncrementalGeneratorInitializationContext context) {
        // Uncomment to debug. It helps, trust me future me.
        // Debugger.Break();

        IncrementalValuesProvider<ClassDeclarationSyntax> classDeclarations = CreateClassDeclarationsProvider(context);
        context.RegisterSourceOutput(
            context.CompilationProvider.Combine(classDeclarations.Collect()),
            (sourceContext, source) => GenerateSources(source.Item1, source.Item2, sourceContext));
    }

    #region ClassDeclarationsProvider
    private static IncrementalValuesProvider<ClassDeclarationSyntax> CreateClassDeclarationsProvider(IncrementalGeneratorInitializationContext context) {
        return context.SyntaxProvider.CreateSyntaxProvider(
            ProviderPredicate,
            ProviderTransform
        );
    }
    private static bool ProviderPredicate(SyntaxNode node, CancellationToken _) {
        return node is ClassDeclarationSyntax { AttributeLists.Count: > 0 };
    }
    private static ClassDeclarationSyntax ProviderTransform(GeneratorSyntaxContext ctx, CancellationToken _) {
        return (ClassDeclarationSyntax)ctx.Node;
    }
    #endregion

    private static void GenerateSources(Compilation compilation, ImmutableArray<ClassDeclarationSyntax> classDeclarations, SourceProductionContext context) {
        INamedTypeSymbol? attributeType = compilation.GetTypeByMetadataName(AttributeMetadataName);
        if (attributeType == null) {
            ReportDiagnostic(context, Rules.NoAttributesFound);
            return;
        }

        List<string> registrations = CollectRegistrations(compilation, classDeclarations, attributeType);
        if (!registrations.Any()) {
            ReportDiagnostic(context, Rules.NoRegistrationsGenerated);
            return;
        }

        string assemblyName = Sanitize(compilation.AssemblyName ?? "Unknown");
        string sourceText = GenerateSourceText(compilation, assemblyName, registrations);
        context.AddSource(GeneratedFileName, SourceText.From(sourceText, Encoding.UTF8));
    }

    private static List<string> CollectRegistrations(Compilation compilation, ImmutableArray<ClassDeclarationSyntax> classDeclarations, INamedTypeSymbol attributeType) {
        var registrations = new List<string>();
        foreach (ClassDeclarationSyntax? candidate in classDeclarations) {
            SemanticModel model = compilation.GetSemanticModel(candidate.SyntaxTree);
            if (model.GetDeclaredSymbol(candidate) is not {} classSymbol) continue;

            foreach (AttributeSyntax? attribute in candidate.AttributeLists.SelectMany(attrList => attrList.Attributes)) {
                if (TryExtractRegistrationInfo(model, attribute, attributeType, classSymbol, out string? registration)) {
                    registrations.Add(registration);
                }
            }
        }
        return registrations;
    }

    private static bool TryExtractRegistrationInfo(SemanticModel model, AttributeSyntax attribute, INamedTypeSymbol attributeType, INamedTypeSymbol classSymbol, out string registration) {
        registration = string.Empty;

        if (model.GetTypeInfo(attribute).Type is not INamedTypeSymbol attributeTypeInfo) return false;
        if (!SymbolEqualityComparer.Default.Equals(attributeTypeInfo.ConstructedFrom, attributeType)) return false;

        if (attribute is not { Name: GenericNameSyntax genericNameSyntax }) return false;
        if (genericNameSyntax.TypeArgumentList.Arguments.FirstOrDefault() is not {} typeArgumentSyntax) return false;
        if (model.GetSymbolInfo(typeArgumentSyntax).Symbol is not INamedTypeSymbol serviceTypeSymbol) return false;
        if (attribute.ArgumentList is not { Arguments.Count: > 0 } attributeArguments) return false;

        ExpressionSyntax? lifetimeArgument = attributeArguments.Arguments.FirstOrDefault()?.Expression;
        if (lifetimeArgument is not MemberAccessExpressionSyntax memberAccessExpression) return false;

        string lifetimeName = memberAccessExpression.ToString()
            .Split('.')
            .Last();
        if (!_validLifeTimes.Contains(lifetimeName)) return false;

        registration = $"services.Add{lifetimeName}<{serviceTypeSymbol.ToDisplayString()}, {classSymbol.ToDisplayString()}>();";
        return true;
    }

    private static string GenerateSourceText(Compilation compilation, string assemblyName, List<string> registrations) {
        StringBuilder sourceBuilder = new StringBuilder()
            .AppendLine("using Microsoft.Extensions.DependencyInjection;")
            .AppendLine($"namespace {compilation.AssemblyName};")
            .AppendLine()
            .AppendLine("public static class InjectableServicesExtensions {")
            .AppendLine($"    public static IServiceCollection RegisterServicesFrom{assemblyName}(this IServiceCollection services) {{");

        registrations.Sort();
        foreach (string? registration in registrations) {
            sourceBuilder.AppendLine($"        {registration}");
        }

        sourceBuilder.AppendLine("        return services;")
            .AppendLine("    }")
            .AppendLine("}");

        return sourceBuilder.ToString();
    }

    private static void ReportDiagnostic(SourceProductionContext context, DiagnosticDescriptor rule) {
        context.ReportDiagnostic(Diagnostic.Create(rule, Location.None));
    }

    private static string Sanitize(string input) => string.Join("", input.Where(char.IsLetterOrDigit));
}
