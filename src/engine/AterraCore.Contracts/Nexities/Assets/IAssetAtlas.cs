// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Diagnostics.CodeAnalysis;
using AterraCore.Common.FlexiPlug;
using AterraCore.Common.Nexities;
namespace AterraCore.Contracts.Nexities.Assets;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------

public interface IAssetAtlas {
    public int TotalCount { get; }

    public bool TryAssignAsset(AssetRegistration registration, [NotNullWhen(true)] out AssetId? assetId);
    public IEnumerable<AssetId> GetAllAssetsOfCoreTag(CoreTags coreTag);
    public IEnumerable<AssetId> GetAllAssetsOfStringTag(string stringTag);
    public IEnumerable<AssetId> GetAllAssetsOfPlugin(PluginId pluginId);

    public bool TryGetType(AssetId assetId, [NotNullWhen(true)] out Type? type);
    public bool TryGetAssetId(Type type, [NotNullWhen(true)] out AssetId? assetId);
}