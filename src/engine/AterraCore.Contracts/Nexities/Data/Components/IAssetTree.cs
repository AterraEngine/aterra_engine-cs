// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Contracts.Nexities.Data.Assets;

namespace AterraCore.Contracts.Nexities.Data.Components;
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
    
    IEnumerable<T> OfTypeUnCached<T>() where T : IAssetInstance;
    IEnumerable<T> OfTypeReverseUnCached<T>() where T : IAssetInstance;
    IEnumerable<T> OfTypeManyUnCached<T>() where T : IAssetInstance;
    IEnumerable<T> OfTypeManyReverseUnCached<T>() where T : IAssetInstance;

    IEnumerable<IAssetInstance> All { get; }
    
    void AddLast<T>(T node) where T : IAssetInstance;
    void AddFirst<T>(T node) where T : IAssetInstance;
}
