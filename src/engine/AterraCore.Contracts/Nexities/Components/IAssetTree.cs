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

    void Add<T>(T node, Ulid? parentUlid = null) where T : IAssetInstance;

    bool TryFind(Ulid assetUlid, [NotNullWhen(true)] out IAssetInstance? output);

    IEnumerable<IAssetInstance> GetChildren(Ulid parentUlid);
    IAssetInstance? GetParent(Ulid assetUlid);
}
