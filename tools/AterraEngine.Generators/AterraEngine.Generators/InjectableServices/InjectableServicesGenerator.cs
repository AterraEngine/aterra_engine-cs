// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading;

namespace AterraEngine.Generators.InjectableServices;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[Generator(LanguageNames.CSharp)]
public class InjectableServicesGenerator : IIncrementalGenerator {
    public const string GeneratedFileName = "InjectableServicesExtensions.g.cs";
    private const string AttributeMetadataName = "AterraEngine.Common.Attributes.InjectableServiceAttribute`1";

    private static readonly ImmutableHashSet<string> ValidLifeTimes = ImmutableHashSet.Create("Singleton", "Scoped", "Transient");

    public void Initialize(IncrementalGeneratorInitializationContext context) {
        IncrementalValueProvider<ImmutableArray<ClassDeclarationSyntax>> syntaxProvider = context.SyntaxProvider
            .CreateSyntaxProvider(ProviderPredicate, ProviderTransform)
            .Collect();

        context.RegisterSourceOutput(context.CompilationProvider.Combine(syntaxProvider), GenerateSources);
    }

    #region ClassDeclarationsProvider
    private static bool ProviderPredicate(SyntaxNode node, CancellationToken _) => node is ClassDeclarationSyntax { AttributeLists.Count: > 0 };
    private static ClassDeclarationSyntax ProviderTransform(GeneratorSyntaxContext ctx, CancellationToken _) => (ClassDeclarationSyntax)ctx.Node;
    #endregion

    #region SourceGenerator
    private static void GenerateSources(SourceProductionContext context, (Compilation, ImmutableArray<ClassDeclarationSyntax>) source) {
        (Compilation? compilation, ImmutableArray<ClassDeclarationSyntax> classDeclarations) = source;

        if (compilation.AssemblyName is not { } assemblyName) {
            ReportDiagnostic(context, Rules.NoAssemblyNameFound);
            return;
        }

        if (compilation.GetTypeByMetadataName(AttributeMetadataName) is not { } attributeType) {
            ReportDiagnostic(context, Rules.NoAttributesFound);
            return;
        }

        IEnumerable<InjectableServiceRegistration> registrations = GetRegistrations(compilation, classDeclarations, attributeType)
            .OrderBy(r => r.LifeTime, StringComparer.InvariantCultureIgnoreCase);

        context.AddSource(
            GeneratedFileName,
            SourceText.From(GenerateSourceText(context, Sanitize(assemblyName), registrations), Encoding.UTF8)
        );
    }

    private static IEnumerable<InjectableServiceRegistration> GetRegistrations(Compilation compilation, ImmutableArray<ClassDeclarationSyntax> classDeclarations, INamedTypeSymbol attributeType) {
        foreach (ClassDeclarationSyntax candidate in classDeclarations) {
            SemanticModel model = compilation.GetSemanticModel(candidate.SyntaxTree);
            if (model.GetDeclaredSymbol(candidate) is not {} implementationTypeSymbol) continue;

            foreach (AttributeSyntax attribute in candidate.AttributeLists.SelectMany(attrList => attrList.Attributes)) {
                if (model.GetTypeInfo(attribute).Type is not INamedTypeSymbol attributeTypeInfo) continue;
                if (!SymbolEqualityComparer.Default.Equals(attributeTypeInfo.ConstructedFrom, attributeType)) continue;
                if (attribute is not { Name: GenericNameSyntax genericNameSyntax }) continue;
                if (genericNameSyntax.TypeArgumentList.Arguments.FirstOrDefault() is not { } typeArgumentSyntax) continue;
                if (model.GetSymbolInfo(typeArgumentSyntax).Symbol is not INamedTypeSymbol serviceTypeSymbol) continue;
                if (attribute.ArgumentList?.Arguments.FirstOrDefault()?.Expression is not MemberAccessExpressionSyntax lifetimeExpr) continue;

                string lifetimeName = lifetimeExpr.Name.Identifier.Text;
                if (!ValidLifeTimes.Contains(lifetimeName)) continue;

                yield return new InjectableServiceRegistration(
                    serviceTypeSymbol.ToDisplayString(),
                    implementationTypeSymbol.ToDisplayString(),
                    lifetimeName
                );
            }
        }
    }

    private static string GenerateSourceText(SourceProductionContext context, string assemblyName, IEnumerable<InjectableServiceRegistration> registrations) {
        StringBuilder sourceBuilder = new StringBuilder()
            .AppendLine("using Microsoft.Extensions.DependencyInjection;")
            .AppendLine($"namespace {assemblyName};")
            .AppendLine()
            .AppendLine("public static class InjectableServicesExtensions {")
            .AppendLine($"    public static IServiceCollection RegisterServicesFrom{assemblyName}(this IServiceCollection services) {{");
        
        int i = 0; // Dumb solution, but it works
        foreach (InjectableServiceRegistration registration in registrations) {
            sourceBuilder.AppendLine($"        services.Add{registration.LifeTime}<{registration.ServiceTypeName}, {registration.ImplementationTypeName}>();");
            i++;
        }
        if (i == 0) ReportDiagnostic(context,Rules.NoServicesRegistered);

        return sourceBuilder.AppendLine("        return services;")
            .AppendLine("    }")
            .AppendLine("}")
            .ToString();
    }
    #endregion

    #region Helper Methods
    private static void ReportDiagnostic(SourceProductionContext context, DiagnosticDescriptor rule) => context.ReportDiagnostic(Diagnostic.Create(rule, Location.None));
    private static string Sanitize(string input) => new(input.Where(char.IsLetterOrDigit).ToArray());
    #endregion
}
