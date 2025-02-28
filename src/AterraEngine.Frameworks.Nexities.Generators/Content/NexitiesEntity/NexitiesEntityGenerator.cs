// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Frameworks.Nexities.Generators.Helpers;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using static AterraEngine.Frameworks.Nexities.Generators.SymbolNames;

namespace AterraEngine.Frameworks.Nexities.Generators.Content.NexitiesEntity;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// --------------------------------------------------------------/-------------------------------------------------------
[Generator(LanguageNames.CSharp)]
public class NexitiesEntityGenerator : IIncrementalGenerator {

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public void Initialize(IncrementalGeneratorInitializationContext context) {
        // ReSharper disable once SuggestVarOrType_Elsewhere
        var syntaxData = context.SyntaxProvider
            .CreateSyntaxProvider(
                IsNexitiesEntity,
                GetNexitiesEntityDto
            )
            .Collect();

        context.RegisterSourceOutput(context.CompilationProvider.Combine(syntaxData), Output);
    }

    private static bool IsNexitiesEntity(SyntaxNode node, CancellationToken ct = default) {
        if (node is not ClassDeclarationSyntax classSyntax) return false;
        if (classSyntax.BaseList is not {} baseList) return false;
        if (baseList.Types.FirstOrDefault(baseType => baseType.Type.ToString().EndsWith(SymbolNames.NexitiesEntity)) is null) return false;
        return true;

    }

    private static NexitiesEntityDto GetNexitiesEntityDto(GeneratorSyntaxContext syntaxContext, CancellationToken ct = default) {
        var classDeclaration = (ClassDeclarationSyntax)syntaxContext.Node;
        ISymbol classSymbol = syntaxContext.SemanticModel.GetDeclaredSymbol(classDeclaration)!;

        // Some symbols will be needed to check to finally validate the DTO
        //      These symbols have to be prepped beforehand
        CachedSymbolConvertor symbolConvertor = [
            NexitiesEntity_DisplayString,
            INexitiesEntity_DisplayString,
            OmniaIdAttribute_DisplayString
        ];

        symbolConvertor.ConvertAll(syntaxContext.SemanticModel.Compilation);
        NexitiesEntityDtoFactory factory = new(symbolConvertor);

        return factory.CreateNew(classDeclaration, classSymbol, ct);
    }

    private static void Output(SourceProductionContext context, (Compilation compilation, ImmutableArray<NexitiesEntityDto> Data) valueTuple) {
        // TODO Do something with the output of the generator
    }
}
