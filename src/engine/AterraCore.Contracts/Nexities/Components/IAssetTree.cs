// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Contracts.OmniVault.Assets;
using System.Diagnostics.CodeAnalysis;

namespace AterraCore.Contracts.Nexities.Components;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IAssetTree : INexitiesComponent {
    int Count { get; }
    
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    IEnumerable<T> OfType<T>() where T : IAssetInstance;
    IEnumerable<T> OfTypeReverse<T>() where T : IAssetInstance;
    IEnumerable<T> OfTypeMany<T>() where T : IAssetInstance;
    IEnumerable<T> OfTypeManyReverse<T>() where T : IAssetInstance;

    void Add<T>(T node) where T : IAssetInstance;
    void AddFirst<T>(T node) where T : IAssetInstance;
}
