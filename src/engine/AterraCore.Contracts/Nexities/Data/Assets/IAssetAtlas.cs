// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Common.Types.Nexities;
using System.Diagnostics.CodeAnalysis;

namespace AterraCore.Contracts.Nexities.Data.Assets;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IAssetAtlas {
    int TotalCount { get; }

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    bool TryAssignAsset(AssetRegistration registration, [NotNullWhen(true)] out AssetId? assetId);
    IEnumerable<AssetId> GetAllAssetsOfCoreTag(CoreTags coreTag);
    IEnumerable<AssetId> GetAllAssetsOfStringTag(string stringTag);
    IEnumerable<AssetId> GetAllAssetsOfPlugin(string pluginId);

    bool TryGetRegistration(AssetId assetId, out AssetRegistration registration);
    bool TryGetType(AssetId assetId, [NotNullWhen(true)] out Type? type);
    bool TryGetAssetId(Type type, out AssetId assetId);
    bool TryGetAssetId<T>(out AssetId assetId);
    bool TryGetInterfaceTypes(AssetId assetId, out Type[] type);
}
