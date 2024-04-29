// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using AterraCore.Contracts.Nexities.Assets;
using AterraCore.Common;
using AterraCore.Common.FlexiPlug;
using AterraCore.Common.Nexities;
using AterraCore.Contracts.FlexiPlug;
using AterraCore.Contracts.FlexiPlug.Plugin;
using AterraCore.Extensions;
using JetBrains.Annotations;
using Serilog;

namespace AterraCore.Nexities.Assets;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------

[UsedImplicitly]
public class AssetAtlas(ILogger logger, IPluginAtlas pluginAtlas) : IAssetAtlas {
    private ConcurrentDictionary<AssetInstanceType, HashSet<AssetId>> _mapTypeToAssetId = new ConcurrentDictionary<AssetInstanceType, HashSet<AssetId>>().PopulateWithEmpties();
    private ConcurrentDictionary<AssetId, Type> _assetsMultiple = new();
    private ConcurrentDictionary<CoreTags, ConcurrentBag<AssetId>> _coreTagedAssets = new ConcurrentDictionary<CoreTags, ConcurrentBag<AssetId>>().PopulateWithEmpties();
    private ConcurrentDictionary<string, ConcurrentBag<AssetId>> _stringTagedAssets = new();

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public bool TryAssignAsset(AssetRegistration registration, [NotNullWhen(true)] out AssetId? assetId) {
        assetId = null;

        var newAssetId = new AssetId(registration.PluginId, registration.PartialAssetId);
        logger.Information(
            "Assigning asset {AssetId} of Type {AssetTypeName} to plugin {PluginId}", 
            newAssetId, registration.Type.FullName, registration.Type
        );
        
        // Assigns the asset to the dict
        if (!_assetsMultiple.TryAdd(newAssetId, registration.Type)) {
            logger.Warning(
                "Asset with ID: {AssetId} already exists with type {ExistingAssetType}. Cannot assign a new asset with the same ID.",
                newAssetId, _assetsMultiple[newAssetId].FullName
            );
            return false;
        }
        
        assetId = newAssetId;
        
        // After Everything is said and done with the assigning, start assigning the Core tags and string tags
        foreach (CoreTags tag in Enum.GetValues(typeof(CoreTags))) {
            if (!registration.CoreTags.HasFlag(tag)) continue;
            
            _coreTagedAssets[tag].Add(newAssetId);
            logger.Information(
                "Asset with Id: {AssetId} is assigned as a '{TagName}'",
                assetId, tag.ToString()
            );
        }
        
        return true;
    }
    
    public IEnumerable<AssetId> GetAllAssetsOfCoreTag(CoreTags coreTag) =>
        Enum.GetValues<CoreTags>()
            .Where(tag => coreTag.HasFlag(tag))
            .SelectMany(tag => _coreTagedAssets[tag])
            .ToArray();

    public IEnumerable<AssetId> GetAllAssetsOfStringTag(string coreTag) => 
        _stringTagedAssets.TryGetValue(coreTag, out ConcurrentBag<AssetId>? bag) ? bag : [];


    public void TryImportAssetsFromPlugins() {
        foreach (AssetRegistration assetRegistration in pluginAtlas.GetAssetRegistrations()) {
            if (!TryAssignAsset(assetRegistration, out AssetId? assetId)) {
                logger.Warning("Type {Type} could not be assigned as an asset", assetRegistration.Type);
            }
            logger.Information("Type {Type} assigned to AssetId {Id}", assetRegistration.Type, assetId);
        }
    }
}