// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Common.Data;
using AterraCore.Common.Types.Nexities;
using System.Collections.Frozen;
using System.Diagnostics.CodeAnalysis;

namespace AterraCore.Contracts.OmniVault.Assets;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IAssetAtlas {
    FrozenDictionary<AssetId, AssetRegistration> AssetsById {get;}
    FrozenDictionary<Type, AssetId> AssetsByType {get;}
    FrozenDictionary<CoreTags, FrozenSet<AssetId>> CoreTaggedAssets {get;}
    FrozenDictionary<string, FrozenSet<AssetId>> StringTaggedAssets {get;}
    
    int TotalCount { get; }

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    IEnumerable<AssetId> GetAllAssetsOfCoreTag(CoreTags coreTag);
    IEnumerable<AssetId> GetAllAssetsOfStringTag(string stringTag);
    IEnumerable<AssetId> GetAllAssetsOfPlugin(string pluginId);

    Type GetAssetType(AssetId assetId);

    bool TryGetRegistration(AssetId assetId, out AssetRegistration registration);
    bool TryGetAssetType(AssetId assetId, [NotNullWhen(true)] out Type? type);
    bool TryGetAssetId(Type type, out AssetId assetId);
    bool TryGetAssetId<T>(out AssetId assetId);
    bool TryGetInterfaceTypes(AssetId assetId, out IEnumerable<Type> type);
}
