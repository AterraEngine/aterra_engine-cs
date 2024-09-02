// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Common.Data;
using AterraCore.Common.Types.Nexities;
using AterraCore.Contracts.FlexiPlug;
using AterraCore.Contracts.OmniVault.Assets;
using CodeOfChaos.Extensions;
using Extensions;
using JetBrains.Annotations;
using Serilog;
using System.Collections.Frozen;

namespace AterraCore.OmniVault.Assets;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[UsedImplicitly]
public class AssetAtlasPopulator(IPluginAtlas pluginAtlas, ILogger logger, AssetAtlas assetAtlas) : IAssetAtlasPopulator {

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public void PopulateAssetAtlas() {
        Dictionary<AssetId, AssetRegistration> assetsById = new();
        Dictionary<Type, AssetId> assetsByType = new();
        Dictionary<string, HashSet<AssetId>> stringTaggedAssets = new();
        Dictionary<CoreTags, HashSet<AssetId>> coreTaggedAssets = new Dictionary<CoreTags, HashSet<AssetId>>()
            .PopulateWithEmpties<Dictionary<CoreTags, HashSet<AssetId>>, CoreTags, HashSet<AssetId>>();
        
        foreach (AssetRegistration assetRegistration in pluginAtlas.GetAssetRegistrations()) {
            if (!TryAssignAsset(assetsById, assetsByType, stringTaggedAssets, coreTaggedAssets, assetRegistration)) {
                logger.Warning("Type {Type} could not be assigned as an asset", assetRegistration.Type);
            }
        }

        // They can only be internally set, because this populator should only be ran during startup
        assetAtlas.AssetsById = assetsById.ToFrozenDictionary();
        assetAtlas.AssetsByType =assetsByType.ToFrozenDictionary();
        assetAtlas.StringTaggedAssets = stringTaggedAssets.ToFrozenDictionary(pair => pair.Key, pair => pair.Value.ToFrozenSet());
        assetAtlas.CoreTaggedAssets = coreTaggedAssets.ToFrozenDictionary(pair => pair.Key, pair => pair.Value.ToFrozenSet());

    }

    private bool TryAssignAsset(
        Dictionary<AssetId, AssetRegistration> assetsById,
        Dictionary<Type, AssetId> assetsByType,
        Dictionary<string, HashSet<AssetId>> stringTaggedAssets,
        Dictionary<CoreTags, HashSet<AssetId>> coreTaggedAssets,
        AssetRegistration registration
    ) {
        // Assigns the asset to the dict
        if (!assetsById.TryAdd(registration.AssetId, registration)) {
            logger.Warning(
                "Asset with ID: {AssetId} already exists with type {ExistingAssetType}. Cannot assign a new asset with the same ID.",
                registration.AssetId, assetsById[registration.AssetId].Type.FullName
            );
            return false;
        }

        if (!assetsByType.TryAdd(registration.Type, registration.AssetId)) {
            // The reason for this, is the class type is hard linked to an AssetId
            logger.Warning(
                "Asset with ID: {AssetId} Cannot assign a new asset because it's {Type} is already assigned to another asset.",
                registration.AssetId, registration.Type.FullName
            );
            return false;
        }

        foreach (Type interfaceType in registration.InterfaceTypes) {
            // The reason for this, is the class type is softly linked to an AssetId, and can be overwritten
            assetsByType.AddOrUpdate(interfaceType, registration.AssetId);
            logger.Information("Asset {AssetId} linked to the interface of {Type}", registration.AssetId, interfaceType.FullName);
        }

        // After Everything is said and done with the assigning, start assigning the Core tags and string tags
        foreach (CoreTags tag in Enum.GetValuesAsUnderlyingType<CoreTags>()) {
            if (!registration.CoreTags.HasFlag(tag)) continue;
            coreTaggedAssets.TryAdd(tag, []);
            coreTaggedAssets[tag].Add(registration.AssetId);
        }

        foreach (string stringTag in registration.StringTags) {
            stringTaggedAssets.TryAdd(stringTag, []);
            stringTaggedAssets[stringTag].Add(registration.AssetId);
        }

        // Assign overloads
        foreach (AssetId overridableAssetId in registration.OverridableAssetIds) {
            assetsById.AddOrUpdate(overridableAssetId, registration);

            logger.Information(
                "Assigned asset {AssetId} to overwrite {overridableAssetId}",
                registration.AssetId,
                overridableAssetId
            );
        }

        logger.Information(
            "Assigned asset {AssetId} of Type {AssetTypeName}",
            registration.AssetId, registration.Type.FullName
        );
        return true;
    }
}
