// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace AterraEngine.Generators;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
#pragma warning disable RS1038
[Generator(LanguageNames.CSharp)]
#pragma warning restore RS1038
public class InjectableServicesGenerator : IIncrementalGenerator {
    public const string GeneratedFileName = "InjectableServicesExtensions.g.cs";
    public const string AttributeMetadataName = "AterraEngine.Common.Attributes.InjectableServiceAttribute`1";

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public void Initialize(IncrementalGeneratorInitializationContext context) {
        // Uncomment to debug
        // Debugger.Break();
        
        // Combine syntax collection and semantic processing in a pipeline
        IncrementalValuesProvider<ClassDeclarationSyntax> classDeclarations =
            context.SyntaxProvider.CreateSyntaxProvider(
                predicate: static (node, _) => node is ClassDeclarationSyntax { AttributeLists.Count: > 0 },
                transform: static (ctx, _) => (ClassDeclarationSyntax)ctx.Node
            );
        
        context.RegisterSourceOutput(
            context.CompilationProvider.Combine(classDeclarations.Collect()), 
            action: (sourceContext, source) => Generate(source.Item1, source.Item2, sourceContext));
    }

    private static void Generate(Compilation compilation, ImmutableArray<ClassDeclarationSyntax> classDeclarations, SourceProductionContext context) {
        if (compilation.GetTypeByMetadataName(AttributeMetadataName) is not {} attributeSymbol) {
            context.ReportDiagnostic(Diagnostic.Create(new DiagnosticDescriptor(
                    "SRVGEN001",
                    "Service Generator Error",
                    "InjectableServiceAttribute not found",
                    "SourceGenerator",
                    DiagnosticSeverity.Warning,
                    true),
                Location.None));
            return;
        }

        var registrations = new List<string>();

        foreach (ClassDeclarationSyntax candidate in classDeclarations) {
            SemanticModel model = compilation.GetSemanticModel(candidate.SyntaxTree);
            if (model.GetDeclaredSymbol(candidate) is not {} classSymbol) continue;

            foreach (AttributeSyntax attribute in candidate.AttributeLists.SelectMany(attrList => attrList.Attributes)) {
                TypeInfo attributeTypeInfo = model.GetTypeInfo(attribute);
                if (attributeTypeInfo.Type is not INamedTypeSymbol typeSymbol) continue;
                if (!SymbolEqualityComparer.Default.Equals(typeSymbol.ConstructedFrom, attributeSymbol)) continue;

                // Extract the type argument (TService)
                if (attribute is not { Name: GenericNameSyntax genericNameSyntax }) continue;
                if (genericNameSyntax.TypeArgumentList.Arguments.FirstOrDefault() is not {} typeArgumentSyntax) continue;
                if (model.GetSymbolInfo(typeArgumentSyntax).Symbol is not INamedTypeSymbol typeArgumentSymbol) continue;

                // Extract the service lifetime argument
                if (attribute.ArgumentList is not { Arguments.Count: > 0 } attributeArguments) continue;

                ExpressionSyntax? lifetimeArgument = attributeArguments.Arguments.FirstOrDefault()?.Expression;
                if (lifetimeArgument is not MemberAccessExpressionSyntax memberAccessExpression) continue;

                // Get the lifetime value from the string representation (e.g., "ServiceLifetime.Transient")
                string lifetimeString = memberAccessExpression.ToString();
                string? lifetime = lifetimeString switch {
                    "ServiceLifetime.Singleton" => "Singleton",
                    "ServiceLifetime.Scoped" => "Scoped",
                    "ServiceLifetime.Transient" => "Transient",
                    _ => null
                };

                if (lifetime is null) continue;

                registrations.Add($"services.Add{lifetime}<{typeArgumentSymbol.ToDisplayString()}, {classSymbol.ToDisplayString()}>();");
            }
        }

        if (registrations.Count <= 0) {
            context.ReportDiagnostic(Diagnostic.Create(new DiagnosticDescriptor(
                "SRVGEN002",
                "Service Generator Info",
                "No registrations were generated",
                "SourceGenerator",
                DiagnosticSeverity.Info,
                true), Location.None));
            return;
        }

        string assemblyName = Sanitize(compilation.AssemblyName ?? "Unknown");

        StringBuilder sourceBuilder = new StringBuilder()
            .AppendLine("using Microsoft.Extensions.DependencyInjection;")
            .AppendLine($"namespace {compilation.AssemblyName};")
            .AppendLine()
            .AppendLine("public static class InjectableServicesExtensions {")
            .AppendLine($"    public static IServiceCollection RegisterServicesFrom{assemblyName}(this IServiceCollection services) {{");

        registrations.Sort();
        foreach (string registration in registrations) {
            sourceBuilder.AppendLine($"        {registration}");
        }

        sourceBuilder.AppendLine("        return services;")
            .AppendLine("    }")
            .AppendLine("}");

        context.AddSource(GeneratedFileName, SourceText.From(sourceBuilder.ToString(), Encoding.UTF8));
    }

    // -----------------------------------------------------------------------------------------------------------------
    // Helpers
    // -----------------------------------------------------------------------------------------------------------------
    private static string Sanitize(string input) => string.Join("", input.Where(c => char.IsLetterOrDigit(c) || c == '_'));
}
