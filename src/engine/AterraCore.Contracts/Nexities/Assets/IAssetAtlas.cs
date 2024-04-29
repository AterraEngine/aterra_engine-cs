// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using AterraCore.Common;
using AterraCore.Contracts.FlexiPlug.Plugin;

namespace AterraCore.Contracts.Nexities.Assets;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------

public interface IAssetAtlas {
    public bool TryAssignAsset(Type assetType, IPluginDto pluginData, IAssetAttribute assetAttribute, [NotNullWhen(true)] out AssetId? assetId);
    public IEnumerable<AssetId> GetAllAssetsOfCoreTag(CoreTags coreTag);
}
