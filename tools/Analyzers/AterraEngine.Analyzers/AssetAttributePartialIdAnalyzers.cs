// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using JetBrains.Annotations;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;

namespace AterraEngine.Analyzers;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public record CacheKey(string AssemblyName, string PartialId);

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[DiagnosticAnalyzer(LanguageNames.CSharp)]
[UsedImplicitly]
public class AssetAttributePartialIdAnalyzers : DiagnosticAnalyzer {
    private const string DiagnosticId = "AE0001";
    private const string Category = "AssetId";

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

    private static void AnalyzeSymbol(SymbolAnalysisContext context) {
        var cachePartialIds = new ConcurrentDictionary<CacheKey, List<Location>>();
        INamespaceSymbol globalNamespace = context.Compilation.GlobalNamespace;

        IEnumerable<AttributeData> attributes = globalNamespace
            .GetMembers()
            .SelectMany(GetAllSymbols)
            .OfType<INamedTypeSymbol>()
            .SelectMany(typeSymbol => typeSymbol.GetAttributes())
            .Where(data => data.AttributeClass != null &&
                           InheritsFrom(data.AttributeClass, "AterraCore.Attributes.AssetAttribute"));

        foreach (AttributeData attribute in attributes) {
            TypedConstant instanceTypeArg = GetInstanceTypeArg(attribute);

            if (instanceTypeArg.Value is not string partialId) continue;

            var recordKey = new CacheKey(context.Compilation.Assembly.Name, partialId.Replace("-", ""));
            Location? location = attribute.ApplicationSyntaxReference?.GetSyntax().GetLocation();

            if (location == null) continue;

            List<Location> foundLocations = cachePartialIds.GetOrAdd(recordKey, [location]);
            if (!foundLocations.Contains(location)) foundLocations.Add(location);
        }

        ReportDiagnostics(context, cachePartialIds);
    }

    private static TypedConstant GetInstanceTypeArg(AttributeData attribute) {
        TypedConstant namedArg = attribute.NamedArguments.FirstOrDefault(na => na.Key == "assetId").Value;
        return namedArg.IsNull ? attribute.ConstructorArguments.FirstOrDefault() : namedArg;
    }

    private static void ReportDiagnostics(SymbolAnalysisContext context, ConcurrentDictionary<CacheKey, List<Location>> cachePartialIds) {
        foreach (KeyValuePair<CacheKey, List<Location>> pair in cachePartialIds.Where(pair => pair.Value.Count > 1)) {
            foreach (Location location in pair.Value) {
                string additionalLocations = string.Join(
                    " ",
                    pair.Value
                        .Where(loc => loc != location)
                        .Select(loc => $"{loc.SourceTree?.FilePath}:{loc.GetLineSpan().StartLinePosition.Line + 1}")
                );

                context.ReportDiagnostic(Diagnostic.Create(
                    Rule,
                    location,
                    pair.Key.PartialId,
                    additionalLocations
                ));
            }
        }
    }

    private static bool InheritsFrom(ITypeSymbol symbol, string baseTypeFullName) {
        for (ITypeSymbol? baseType = symbol; baseType != null; baseType = baseType.BaseType)
            if (baseType.ToDisplayString().Equals(baseTypeFullName)) return true;
        return false;
    }

    private static IEnumerable<ISymbol> GetAllSymbols(INamespaceOrTypeSymbol rootSymbol) {
        var symbolsStack = new Stack<INamespaceOrTypeSymbol>();
        symbolsStack.Push(rootSymbol);

        while (symbolsStack.TryPop(out INamespaceOrTypeSymbol? symbol)) {
            foreach (ISymbol member in symbol.GetMembers()) {
                yield return member;
                if (member is INamespaceOrTypeSymbol namespaceOrTypeSymbol) {
                    symbolsStack.Push(namespaceOrTypeSymbol);
                }
            }
        }
    }
}
