// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace AterraCore.Contracts.Nexities.Data.Assets;

using AterraCore.Common.FlexiPlug;
using AterraCore.Common.Nexities;
using System.Diagnostics.CodeAnalysis;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------

public interface IAssetAtlas {
    public int TotalCount { get; }

    public bool TryAssignAsset(AssetRegistration registration, [NotNullWhen(true)] out AssetId? assetId);
    public IEnumerable<AssetId> GetAllAssetsOfCoreTag(CoreTags coreTag);
    public IEnumerable<AssetId> GetAllAssetsOfStringTag(string stringTag);
    public IEnumerable<AssetId> GetAllAssetsOfPlugin(PluginId pluginId);

    public bool TryGetRegistration(AssetId assetId, out AssetRegistration registration);
    public bool TryGetType(AssetId assetId, [NotNullWhen(true)] out Type? type);
    public bool TryGetAssetId(Type type, out AssetId assetId);
    public bool TryGetAssetId<T>(out AssetId assetId);
    public bool TryGetInterfaceType(AssetId assetId, out Type? type);
}