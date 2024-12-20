// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace AterraEngine.Frameworks.Nexities.Generators.Content.NexitiesEntity;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public struct NexitiesEntityDto() {
    public bool IsEmpty { get; private set; } = false;
    public bool IsInvalid => Issues != NexitiesEntityIssues.Undefined;
    public NexitiesEntityIssues Issues { get; private set; } = NexitiesEntityIssues.Undefined;

    public ClassDeclarationSyntax ClassDeclarationSyntax { get; set; } = null!;
    public ISymbol ClassSymbol { get; set; } = null!;
    
    public Location Location => ClassDeclarationSyntax.GetLocation();

    // -----------------------------------------------------------------------------------------------------------------
    // Constructors
    // -----------------------------------------------------------------------------------------------------------------
    public static NexitiesEntityDto Empty = new() {IsEmpty = true};
    public static NexitiesEntityDto AsEmpty(
        ClassDeclarationSyntax classDeclaration, 
        ISymbol classSymbol,
        NexitiesEntityIssues issue = NexitiesEntityIssues.Undefined
    ) => new() {
        IsEmpty = true,
        Issues = issue,
        ClassDeclarationSyntax = classDeclaration,
        ClassSymbol = classSymbol
    };
}
