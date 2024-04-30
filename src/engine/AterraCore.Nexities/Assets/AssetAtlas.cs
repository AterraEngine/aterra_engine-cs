// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using AterraCore.Common.FlexiPlug;
using AterraCore.Contracts.Nexities.Assets;
using AterraCore.Common.Nexities;
using AterraCore.Contracts.FlexiPlug;
using AterraCore.Extensions;
using JetBrains.Annotations;
using Serilog;

namespace AterraCore.Nexities.Assets;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------

[UsedImplicitly]
public class AssetAtlas(ILogger logger, IPluginAtlas pluginAtlas) : IAssetAtlas {
    private ConcurrentDictionary<AssetInstanceType, ConcurrentBag<AssetId>> _assetIntanceTypeMap = new ConcurrentDictionary<AssetInstanceType, ConcurrentBag<AssetId>>().PopulateWithEmpties();
    private ConcurrentDictionary<AssetId, Type> _assets = new();
    
    private ConcurrentDictionary<CoreTags, ConcurrentBag<AssetId>> _coreTagedAssets = new ConcurrentDictionary<CoreTags, ConcurrentBag<AssetId>>().PopulateWithEmpties();
    private ConcurrentDictionary<string, ConcurrentBag<AssetId>> _stringTagedAssets = new();

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public bool TryAssignAsset(AssetRegistration registration, [NotNullWhen(true)] out AssetId? assetId) {
        assetId = null;

        var newAssetId = new AssetId(registration.PluginId, registration.PartialAssetId);

        // Assigns the asset to the dict
        if (!_assets.TryAdd(newAssetId, registration.Type)) {
            logger.Warning(
                "Asset with ID: {AssetId} already exists with type {ExistingAssetType}. Cannot assign a new asset with the same ID.",
                newAssetId, _assets[newAssetId].FullName
            );
            return false;
        }
        
        // Assign to the instance type
        if (!_assetIntanceTypeMap.TryAddToBagOrCreateBag(registration.InstanceType, newAssetId)) {
            // This shouldn't happen
            logger.Error("Asset {AssetId} could not be be mapped to an InstanceType", newAssetId);
        }
        
        // After Everything is said and done with the assigning, start assigning the Core tags and string tags
        List<string> tagsList = [];
        
        foreach (CoreTags tag in Enum.GetValuesAsUnderlyingType<CoreTags>()) {
            if (!registration.CoreTags.HasFlag(tag)) continue;
            _coreTagedAssets.TryAddToBagOrCreateBag(tag, newAssetId);
            tagsList.Add(tag.ToString());
        }

        foreach (string stringTag in registration.StringTags) {
            if(!_stringTagedAssets.TryAddToBagOrCreateBag(stringTag, newAssetId)){
                logger.Warning("String Tag of {tag} could not be assigned to {assetId}", stringTag, newAssetId);
                continue;
            }
            tagsList.Add(stringTag);
        }
        
        logger.Information(
            "Assigned asset {AssetId} of Type {AssetTypeName} to plugin {PluginId} as a '{@tags}", 
            newAssetId, registration.Type.FullName, registration.Type, tagsList
        );

        assetId = newAssetId;
        return true;
    }
    
    public IEnumerable<AssetId> GetAllAssetsOfCoreTag(CoreTags coreTag) =>
        Enum.GetValues<CoreTags>()
            .Where(tag => coreTag.HasFlag(tag))
            .SelectMany(tag => _coreTagedAssets[tag])
            .ToArray();

    public IEnumerable<AssetId> GetAllAssetsOfStringTag(string stringTag) => 
        _stringTagedAssets.TryGetValue(stringTag, out ConcurrentBag<AssetId>? bag) ? bag : [];

    public IEnumerable<AssetId> GetAllAssetsOfPlugin(PluginId pluginId) =>
        _assets.Where(pair => pair.Key.PluginId == pluginId).Select(pair => pair.Key);
    
    public bool TryGetType(AssetId assetId, [NotNullWhen(true)] out Type? type) {
        return _assets.TryGetValue(assetId, out type);
    }
     
    public bool TryGetAssetId(Type type, [NotNullWhen(true)] out AssetId? assetId) {
        assetId = null;
        
        try {
            KeyValuePair<AssetId, Type> pair = _assets.First(pair => pair.Value == type);
            assetId = pair.Key;
            return true;
        }
        catch (Exception e) when (e is ArgumentNullException or InvalidOperationException){
            logger.Warning("Type {Type} is not linked to an AssetId", type);
            return false;
        }
    }

    public void TryImportAssetsFromPlugins() {
        foreach (AssetRegistration assetRegistration in pluginAtlas.GetAssetRegistrations()) {
            if (!TryAssignAsset(assetRegistration, out AssetId? _)) {
                logger.Warning("Type {Type} could not be assigned as an asset", assetRegistration.Type);
            }
        }
    }
}