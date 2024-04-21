// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using AterraCore.Contracts.FlexiPlug;
using AterraCore.Common;

namespace AterraCore.Contracts.Nexities.Assets;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------

public interface IAssetAtlas {
    public bool TryAssignAsset(Type assetType, IPluginData pluginData, IAssetAttribute assetAttribute, [NotNullWhen(true)] out AssetId? assetId);
    public IEnumerable<AssetId> GetAllAssetsOfCoreTag(CoreTags coreTag);
}
