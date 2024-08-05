﻿// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Contracts.OmniVault.Assets;
using System.Diagnostics.CodeAnalysis;

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

    bool TryGetFirst<T>([NotNullWhen(true)] out T? output) where T : class, IAssetInstance;
    IAssetInstance? First { get; }

    bool TryGetLast<T>([NotNullWhen(true)] out T? output) where T : class, IAssetInstance;
    IAssetInstance? Last { get; }

    bool TryAddAfter<T>(Guid assetGuid, T newAsset) where T : IAssetInstance;
    bool TryAddBefore<T>(Guid assetGuid, T newAsset) where T : IAssetInstance;

    bool TryFind(Guid assetGuid, [NotNullWhen(true)] out LinkedListNode<IAssetInstance>? output);
    bool TryFind(Guid assetGuid, [NotNullWhen(true)] out IAssetInstance? output);
    bool TryFind<T>(Guid assetGuid, [NotNullWhen(true)] out T? output) where T : class, IAssetInstance;
}
