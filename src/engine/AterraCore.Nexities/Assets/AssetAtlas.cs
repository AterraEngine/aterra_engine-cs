// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using AterraCore.Contracts.FlexiPlug;
using AterraCore.Contracts.Nexities.Assets;
using AterraCore.Common;
using Serilog;

namespace AterraCore.Nexities.Assets;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------

public class AssetAtlas(ILogger logger) : IAssetAtlas {
    private ConcurrentDictionary<AssetId, Type> _assetsMultiple = new();
    private ConcurrentDictionary<CoreTags, ConcurrentBag<AssetId>> _coreTagedAssets = new(
        Enum.GetValues(typeof(CoreTags))
            .Cast<CoreTags>()
            .ToDictionary(tag => tag, _ => new ConcurrentBag<AssetId>())
    );
    private ConcurrentDictionary<string, ConcurrentBag<AssetId>> _stringTagedAssets = new();

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public bool TryAssignAsset(Type assetType, IPluginData pluginData, IAssetAttribute assetAttribute, [NotNullWhen(true)] out AssetId? assetId) {
        assetId = null;

        var newAssetId = new AssetId(pluginData.Id, assetAttribute.PartialAssetId);
        logger.Information(
            "Assigning asset {AssetId} of Type {AssetTypeName} to plugin {PluginId}", 
            newAssetId, assetType.FullName, pluginData.Id
        );
        
        // Assigns the asset to the dict
        if (!_assetsMultiple.TryAdd(newAssetId, assetType)) {
            logger.Warning(
                "Asset with ID: {AssetId} already exists with type {ExistingAssetType}. Cannot assign a new asset with the same ID.",
                newAssetId, _assetsMultiple[newAssetId]
            );
            return false;
        }
        assetId = newAssetId;
        
        // After Everything is said and done with the assigning, start assigning the Core tags and string tags
        foreach (CoreTags tag in Enum.GetValues(typeof(CoreTags))) {
            if (!assetAttribute.CoreTags.HasFlag(tag)) continue;
            
            _coreTagedAssets[tag].Add(newAssetId);
            logger.Information(
                "Asset with Id: {AssetId} is assigned as a {TagName}",
                assetId, tag.ToString()
            );
        }
    
        return true;
    }

    public IEnumerable<AssetId> GetAllAssetsOfCoreTag(CoreTags coreTag) {
        return Enum.GetValues<CoreTags>()
            .Where(tag => coreTag.HasFlag(tag))
            .SelectMany(tag => _coreTagedAssets[tag])
            .ToArray();
    }
    
    public IEnumerable<AssetId> GetAllAssetsOfStringTag(string coreTag) {
        return _stringTagedAssets.TryGetValue(coreTag, out ConcurrentBag<AssetId>? bag) ? bag : [];
    }
    
}