// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Common.Types.Nexities;
using AterraCore.Contracts.Nexities.Data.Assets;
using AterraCore.Contracts.Nexities.Data.Components;
using System.Diagnostics.CodeAnalysis;

namespace AterraCore.Contracts.Nexities.Data.Entities;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface INexitiesEntity : IAssetInstance {
    IComponent[] Components { get; }
    AssetId[] ComponentAssetIds { get; }

    bool TryGetComponent(AssetId assetId, [NotNullWhen(true)] out IComponent? component);
    bool TryAddComponent(IComponent component);
    bool TryUpdateComponent(IComponent component);
    bool TryUpdateComponent(IComponent component, [NotNullWhen(true)] out IComponent? oldComponent);
    bool TryRemoveComponent(AssetId assetId, [NotNullWhen(true)] out IComponent? oldComponent);
    bool TryRemoveComponent(AssetId assetId);
}
