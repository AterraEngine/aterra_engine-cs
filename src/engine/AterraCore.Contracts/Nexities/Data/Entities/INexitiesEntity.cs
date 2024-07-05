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
    public IComponent[] Components { get; }
    public AssetId[] ComponentAssetIds { get; }

    public bool TryGetComponent(AssetId assetId, [NotNullWhen(true)] out IComponent? component);
    public bool TryAddComponent(IComponent component);
    public bool TryUpdateComponent(IComponent component);
    public bool TryUpdateComponent(IComponent component, [NotNullWhen(true)] out IComponent? oldComponent);
    public bool TryRemoveComponent(AssetId assetId, [NotNullWhen(true)] out IComponent? oldComponent);
    public bool TryRemoveComponent(AssetId assetId);
}
