// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using Microsoft.CodeAnalysis;

namespace AterraEngine.Frameworks.Nexities.Generators.Helpers;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public static class CachedSymbolConvertorExtensions {
    public static INamedTypeSymbol GetNexitiesEntity(this CachedSymbolConvertor self) {
        INamedTypeSymbol output = self.GetSymbol(SymbolNames.NexitiesEntity_DisplayString);
        return output;
    }

    public static bool EqualsNexitiesEntity(this CachedSymbolConvertor self, INamedTypeSymbol? left) {
        return left is not null && self.EqualsSymbol(left, SymbolNames.NexitiesEntity_DisplayString);
    }
    
    public static INamedTypeSymbol GetOmniaId(this CachedSymbolConvertor self) {
        INamedTypeSymbol output = self.GetSymbol(SymbolNames.OmniaIdAttribute_DisplayString);
        return output;
    }

    public static bool EqualsOmniaId(this CachedSymbolConvertor self, INamedTypeSymbol? left) {
        return left is not null && self.EqualsSymbol(left, SymbolNames.OmniaIdAttribute_DisplayString);
    }
}
