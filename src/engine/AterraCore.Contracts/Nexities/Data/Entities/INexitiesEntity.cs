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
    ICollection<INexitiesComponent> Components { get; }
    ICollection<AssetId> ComponentAssetIds { get; }

    bool TryGetComponent(AssetId assetId, [NotNullWhen(true)] out INexitiesComponent? component);
    bool TryAddComponent(INexitiesComponent component);
    bool TryUpdateComponent(INexitiesComponent component);
    bool TryUpdateComponent(INexitiesComponent component, [NotNullWhen(true)] out INexitiesComponent? oldComponent);
    bool TryRemoveComponent(AssetId assetId, [NotNullWhen(true)] out INexitiesComponent? oldComponent);
    bool TryRemoveComponent(AssetId assetId);
}
