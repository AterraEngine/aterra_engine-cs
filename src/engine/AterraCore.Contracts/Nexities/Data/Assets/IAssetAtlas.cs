// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Common.Types.FlexiPlug;
using AterraCore.Common.Types.Nexities;
using System.Diagnostics.CodeAnalysis;

namespace AterraCore.Contracts.Nexities.Data.Assets;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------

public interface IAssetAtlas {
    public int TotalCount { get; }

    public bool TryAssignAsset(AssetRegistration registration, [NotNullWhen(true)] out AssetId? assetId);
    public IEnumerable<AssetId> GetAllAssetsOfCoreTag(CoreTags coreTag);
    public IEnumerable<AssetId> GetAllAssetsOfStringTag(string stringTag);
    public IEnumerable<AssetId> GetAllAssetsOfPlugin(string pluginId);

    public bool TryGetRegistration(AssetId assetId, out AssetRegistration registration);
    public bool TryGetType(AssetId assetId, [NotNullWhen(true)] out Type? type);
    public bool TryGetAssetId(Type type, out AssetId assetId);
    public bool TryGetAssetId<T>(out AssetId assetId);
    public bool TryGetInterfaceType(AssetId assetId, out Type? type);
}
