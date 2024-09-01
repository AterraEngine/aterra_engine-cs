// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Common.Types.Nexities;
using AterraCore.Contracts.Nexities.Components;
using AterraCore.Contracts.OmniVault.Assets;
using System.Diagnostics.CodeAnalysis;

namespace AterraCore.Contracts.Nexities.Entities;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface INexitiesEntity : IAssetInstance {
    IReadOnlyCollection<INexitiesComponent> Components { get; }
    IReadOnlyCollection<AssetId> ComponentAssetIds { get; }
    public IReadOnlyCollection<Ulid> ComponentInstanceIds { get; }

    T GetComponent<T>(AssetId assetId) where T : INexitiesComponent;
    T GetComponent<T>() where T : INexitiesComponent;

    bool TryGetComponent<T>([NotNullWhen(true)] out T? component) where T : INexitiesComponent;
    bool TryGetComponent<T>(AssetId assetId, [NotNullWhen(true)] out T? component) where T : INexitiesComponent;
    bool TryGetComponent(AssetId assetId, [NotNullWhen(true)] out INexitiesComponent? component);
    bool TryAddComponent(INexitiesComponent component);
    bool TryOverwriteComponent(INexitiesComponent component);
    bool TryOverwriteComponent(INexitiesComponent component, [NotNullWhen(true)] out INexitiesComponent? oldComponent);
}
