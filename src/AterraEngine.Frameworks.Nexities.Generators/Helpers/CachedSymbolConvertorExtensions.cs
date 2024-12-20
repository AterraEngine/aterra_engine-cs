// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Frameworks.Nexities.Generators.Content.NexitiesEntity;
using AterraEngine.Frameworks.Nexities.Generators.Content.NexitiesTag;
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
    public static INamedTypeSymbol GetNexitiesTag(this CachedSymbolConvertor self) {
        INamedTypeSymbol output = self.GetSymbol(SymbolNames.NexitiesTag_DisplayString);
        return output;
    }

    public static bool EqualsNexitiesTag(this CachedSymbolConvertor self, INamedTypeSymbol? left) {
        return left is not null && self.EqualsSymbol(left, SymbolNames.NexitiesTag_DisplayString);
    }
}
