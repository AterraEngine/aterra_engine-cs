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
    
    IEnumerable<T> OfTypeLocal<T>() where T : IAssetInstance;
    IEnumerable<T> OfTypeReverseLocal<T>() where T : IAssetInstance;
    IEnumerable<T> OfTypeManyLocal<T>() where T : IAssetInstance;
    IEnumerable<T> OfTypeManyReverseLocal<T>() where T : IAssetInstance;
    
    void AddLast<T>(T node) where T : IAssetInstance;
    void AddFirst<T>(T node) where T : IAssetInstance;

    bool TryGetFirst<T>([NotNullWhen(true)] out T? output) where T : class, IAssetInstance;
    IAssetInstance? First { get; }

    bool TryGetLast<T>([NotNullWhen(true)] out T? output) where T : class, IAssetInstance;
    IAssetInstance? Last { get; }

    bool TryAddAfter<T>(Ulid assetUlid, T newAsset) where T : IAssetInstance;
    bool TryAddBefore<T>(Ulid assetUlid, T newAsset) where T : IAssetInstance;

    bool TryFind(Ulid assetUlid, [NotNullWhen(true)] out LinkedListNode<IAssetInstance>? output);
    bool TryFind(Ulid assetUlid, [NotNullWhen(true)] out IAssetInstance? output);
    bool TryFind<T>(Ulid assetUlid, [NotNullWhen(true)] out T? output) where T : class, IAssetInstance;
}
