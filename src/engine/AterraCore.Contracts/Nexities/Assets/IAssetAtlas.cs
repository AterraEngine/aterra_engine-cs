// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Diagnostics.CodeAnalysis;
using AterraCore.Common.Nexities;
using AterraCore.Contracts.FlexiPlug.Plugin;

namespace AterraCore.Contracts.Nexities.Assets;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------

public interface IAssetAtlas {
    public bool TryAssignAsset(AssetRegistration registration, [NotNullWhen(true)] out AssetId? assetId);
    public IEnumerable<AssetId> GetAllAssetsOfCoreTag(CoreTags coreTag);
    public void TryImportAssetsFromPlugins();
}
