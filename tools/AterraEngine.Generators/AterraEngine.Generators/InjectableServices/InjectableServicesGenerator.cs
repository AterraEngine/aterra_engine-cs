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
    public const string AttributeMetadataName = "AterraEngine.Common.Attributes.InjectableServiceAttribute`1";

    private static readonly ImmutableHashSet<string> ValidLifeTimes = ["Singleton", "Scoped", "Transient"];
    
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public void Initialize(IncrementalGeneratorInitializationContext context) {
        context.RegisterSourceOutput(
            context.CompilationProvider.Combine(
                context.SyntaxProvider.CreateSyntaxProvider(
                    ProviderPredicate,
                    ProviderTransform
                )
                .Where(syntax => syntax is not null)
                .Collect()
            ),
            (sourceContext, source) => GenerateSources(source.Item1, source.Item2, sourceContext));
    }

    #region ClassDeclarationsProvider
    private static bool ProviderPredicate(SyntaxNode node, CancellationToken _) {
        return node is ClassDeclarationSyntax { AttributeLists.Count: > 0 };
    }
    private static ClassDeclarationSyntax ProviderTransform(GeneratorSyntaxContext ctx, CancellationToken _) {
        return (ClassDeclarationSyntax)ctx.Node;
    }
    #endregion

    private static void GenerateSources(Compilation compilation, ImmutableArray<ClassDeclarationSyntax> classDeclarations, SourceProductionContext context) {
        if (compilation.AssemblyName is not { } assemblyName) {
            ReportDiagnostic(context, Rules.NoAssemblyNameFound);
            return;
        }
        
        if (compilation.GetTypeByMetadataName(AttributeMetadataName) is not { } attributeType) {
            ReportDiagnostic(context, Rules.NoAttributesFound);
            return;
        }

        List<InjectableServiceRegistration> registrations = GetRegistrations(compilation, classDeclarations, attributeType).ToList();
        registrations.Sort((registration1, registration2) => 
            StringComparer.InvariantCultureIgnoreCase.Compare(registration1.LifeTime, registration2.LifeTime)
        );
        
        context.AddSource(
            GeneratedFileName, 
            SourceText.From(GenerateSourceText(Sanitize(assemblyName), registrations), Encoding.UTF8)
        );
    }
    
    private static IEnumerable<InjectableServiceRegistration> GetRegistrations(Compilation compilation, ImmutableArray<ClassDeclarationSyntax> classDeclarations, INamedTypeSymbol attributeType) {
        foreach (ClassDeclarationSyntax candidate in classDeclarations) {
            SemanticModel model = compilation.GetSemanticModel(candidate.SyntaxTree);
            if (model.GetDeclaredSymbol(candidate) is not { } classSymbol) continue;

            foreach (AttributeSyntax attribute in candidate.AttributeLists.SelectMany(attrList => attrList.Attributes)) {
                if (model.GetTypeInfo(attribute).Type is not INamedTypeSymbol attributeTypeInfo) continue;
                if (!SymbolEqualityComparer.Default.Equals(attributeTypeInfo.ConstructedFrom, attributeType)) continue;

                if (attribute is not { Name: GenericNameSyntax genericNameSyntax }) continue;
                if (genericNameSyntax.TypeArgumentList.Arguments.FirstOrDefault() is not { } typeArgumentSyntax) continue;
                if (model.GetSymbolInfo(typeArgumentSyntax).Symbol is not INamedTypeSymbol serviceTypeSymbol) continue;
                if (attribute.ArgumentList is not { Arguments.Count: > 0 } attributeArguments) continue;

                ExpressionSyntax? lifetimeArgument = attributeArguments.Arguments.FirstOrDefault()?.Expression;
                if (lifetimeArgument is not MemberAccessExpressionSyntax memberAccessExpression) continue;

                string lifetimeName = memberAccessExpression.ToString().Split('.').Last();
                if (!ValidLifeTimes.Contains(lifetimeName)) continue;

                yield return new InjectableServiceRegistration(
                    serviceTypeSymbol.ToDisplayString(),
                    classSymbol.ToDisplayString(),
                    lifetimeName
                );
            }
        }
    }

    private static string GenerateSourceText(string assemblyName, IEnumerable<InjectableServiceRegistration> registrations) {
        StringBuilder sourceBuilder = new StringBuilder()
            .AppendLine("using Microsoft.Extensions.DependencyInjection;")
            .AppendLine($"namespace {assemblyName};")
            .AppendLine()
            .AppendLine("public static class InjectableServicesExtensions {")
            .AppendLine($"    public static IServiceCollection RegisterServicesFrom{assemblyName}(this IServiceCollection services) {{");

        
        foreach (InjectableServiceRegistration registration in registrations) {
            sourceBuilder.AppendLine($"        services.Add{registration.LifeTime}<{registration.ServiceTypeName}, {registration.ImplementationTypeName}>();");
        }

        sourceBuilder.AppendLine("        return services;")
            .AppendLine("    }")
            .AppendLine("}");

        return sourceBuilder.ToString();
    }

    #region Helper Methods
    private static void ReportDiagnostic(SourceProductionContext context, DiagnosticDescriptor rule) {
        context.ReportDiagnostic(Diagnostic.Create(rule, Location.None));
    }

    private static string Sanitize(string input) => string.Join("", input.Where(char.IsLetterOrDigit));
    #endregion
}