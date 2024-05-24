// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using AterraEngine.Analyzers.Extensions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
namespace AterraEngine.Analyzers;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public record CacheKey(string AssemblyName, string PartialId);

public record CacheValue(List<Location> locations, string PartialId);

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[DiagnosticAnalyzer(LanguageNames.CSharp)]
public class AssetAttributePartialIdAnalyzers : DiagnosticAnalyzer {
    private const string DiagnosticId = "AE0002";
    private const string Category = "AssetId";

    private static readonly DiagnosticDescriptor Rule = new(
        DiagnosticId,
        new LocalizableResourceString(nameof(Resources.AE0002Title), Resources.ResourceManager, typeof(Resources)),
        new LocalizableResourceString(nameof(Resources.AE0002MessageFormat), Resources.ResourceManager, typeof(Resources)),
        Category,
        DiagnosticSeverity.Error,
        true,
        new LocalizableResourceString(nameof(Resources.AE0002Description), Resources.ResourceManager, typeof(Resources))
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
        ConcurrentDictionary<CacheKey, List<Location>> cachePartialIds = new();

        // Retrieve all the symbols in the global namespace
        IEnumerable<AttributeData> attributes = context.Compilation.GlobalNamespace
            .GetMembers()
            .SelectMany(GetAllSymbolsRecursively)
            .OfType<INamedTypeSymbol>()
            .SelectMany(typeSymbol => typeSymbol.GetAttributes())
            .Where(data => data.AttributeClass != null
                           && InheritsFrom(data.AttributeClass, "AterraCore.Nexities.Assets.AssetAttribute")
            );

        foreach (AttributeData attribute in attributes) {

            // Continue with the name of the argument
            //      WAY TOO UGLY BUT IT WORKS SOMEHOW
            TypedConstant instanceTypeArg = attribute.NamedArguments.Any(na => na.Key == "partialId")
                                            && attribute.NamedArguments.Single(na => na.Key == "partialId").Value.Value is not null
                ? attribute.NamedArguments.Single(na => na.Key == "partialId").Value
                : attribute.ConstructorArguments.DefaultIfEmpty(new TypedConstant()).First();

            var recordKey = new CacheKey(
                context.Compilation.Assembly.Name,
                (instanceTypeArg.Value as string)!.Replace("-", "")
            );
            Location location = attribute.ApplicationSyntaxReference?.GetSyntax().GetLocation()!;

            List<Location> foundLocations = cachePartialIds.GetOrAdd(recordKey, [location]);
            if (!foundLocations.Contains(location)) foundLocations.Add(location);
        }

        cachePartialIds
            .Where(pair => pair.Value.Count > 1)
            .IterateOver(pair => pair.Value.IterateOver(
                location => context.ReportDiagnostic(Diagnostic.Create(
                    Rule,
                    location,
                    pair.Key.PartialId,
                    string.Join(' ', pair.Value.Where(loc => loc != location).Select(loc => loc.SourceTree?.FilePath))
                ))
            ));
    }

    private static bool InheritsFrom(ITypeSymbol symbol, string baseTypeFullName) {
        ITypeSymbol? baseType = symbol;
        while (baseType != null) {
            if (baseType.ToDisplayString().Equals(baseTypeFullName)) return true;
            baseType = baseType.BaseType;
        }
        return false;
    }

    private static IEnumerable<ISymbol> GetAllSymbolsRecursively(INamespaceOrTypeSymbol symbol) {
        foreach (ISymbol memberSymbol in symbol.GetMembers()) {
            if (memberSymbol is INamespaceOrTypeSymbol namespaceOrTypeSymbol)
                foreach (ISymbol child in GetAllSymbolsRecursively(namespaceOrTypeSymbol))
                    yield return child;

            yield return memberSymbol;
        }
    }
}