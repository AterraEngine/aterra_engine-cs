// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Frameworks.Nexities.Generators.Helpers;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using static AterraEngine.Frameworks.Nexities.Generators.SymbolNames;

namespace AterraEngine.Frameworks.Nexities.Generators.Content.NexitiesEntity;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
// ReSharper disable UnusedVariable
// ReSharper disable RedundantJumpStatement
public class NexitiesEntityDtoFactory(CachedSymbolConvertor symbolConvertor) {

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    // ReSharper disable once InvertIf
    public NexitiesEntityDto CreateNew(ClassDeclarationSyntax classSyntax, ISymbol classSymbol, CancellationToken ct = default) {
        // If anything fails, return an empty NexitiesEntityDto
        if (classSymbol is not INamedTypeSymbol namedTypeSymbol) return NexitiesEntityDto.AsEmpty(classSyntax, classSymbol);
        if (classSymbol is not ITypeSymbol typeSymbol) return NexitiesEntityDto.AsEmpty(classSyntax, classSymbol);
        if (!IsNexitiesEntity(typeSymbol)) return NexitiesEntityDto.AsEmpty(classSyntax, classSymbol);
        
        // After we've done basic checks, we shouldn't be using AsEmpty without added context anymore
        //      Now we are in a state when there are issues, they should become diagnostics
        ImmutableArray<AttributeData> attributes = classSymbol.GetAttributes();
        if (!HasNecessaryAttributes(attributes, out AttributeData? tagAttribute)) {
            var issues = NexitiesEntityIssues.Undefined;
            if (tagAttribute is null) issues |= NexitiesEntityIssues.NoTagAttribute;
            return NexitiesEntityDto.AsEmpty(classSyntax, classSymbol, issues);
        }
        
        return new NexitiesEntityDto {
            ClassDeclarationSyntax = classSyntax,
            ClassSymbol = classSymbol
        };
    }
    
    #region IsNexitiesEntity
    // ReSharper disable once ConvertIfStatementToReturnStatement
    public bool IsNexitiesEntity(ITypeSymbol typeSymbol) {
        if (!typeSymbol.AllInterfaces.Any(i => i.ToDisplayString() == INexitiesEntity_DisplayString)) return false;
        if (typeSymbol.BaseType is null) return false;
        
        // TODO A type to check might eventually be a type of a type of a ... of a Nexities Entity, this doesn't do a deep search for this
        if (!symbolConvertor.EqualsNexitiesEntity(typeSymbol.BaseType)) return false;
        return true;
    }
    #endregion
    #region HasNecessaryAttributes
    public bool HasNecessaryAttributes(ImmutableArray<AttributeData> attributes, [NotNullWhen(true)] out AttributeData? tagAttribute ) {
        tagAttribute = null;
        if (attributes.Length == 0) return false; 

        // The reason we aren't creating a specific Has... method for each attribute
        //      is because we don't want to iterate of the array multiple times.
        //      We want to do this once, and then use the result.
        foreach (AttributeData attribute in attributes) {
            if (symbolConvertor.EqualsNexitiesTag(attribute.AttributeClass)) {
                tagAttribute = attribute;
                continue;
            }
            // Add more if statements the more tags we need to find.
        }
        return tagAttribute is not null;
    }
    #endregion
}
