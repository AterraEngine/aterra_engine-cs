// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

namespace AterraCore.Nexities.Assets;

using Common.FlexiPlug;
using AterraCore.Common.Nexities;
using AterraCore.Contracts.Nexities.Data.Assets;
using Extensions;
using JetBrains.Annotations;
using Serilog;
using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------

[UsedImplicitly]
public class AssetAtlas(ILogger logger) : IAssetAtlas {
    private readonly ConcurrentDictionary<ServiceLifetimeType, ConcurrentBag<AssetId>> _assetServiceLifetimeMap = new ConcurrentDictionary<ServiceLifetimeType, ConcurrentBag<AssetId>>().PopulateWithEmpties();
    private readonly ConcurrentDictionary<AssetId, AssetRegistration> _assetsById = new();
    private readonly ConcurrentDictionary<Type, AssetId> _assetsByType = new();

    private readonly ConcurrentDictionary<CoreTags, ConcurrentBag<AssetId>> _coreTaggedAssets = new ConcurrentDictionary<CoreTags, ConcurrentBag<AssetId>>().PopulateWithEmpties();
    private readonly ConcurrentDictionary<string, ConcurrentBag<AssetId>> _stringTaggedAssets = new();

    public int TotalCount => _assetsById.Count;

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public bool TryAssignAsset(AssetRegistration registration, [NotNullWhen(true)] out AssetId? assetId) {
        assetId = null;

        var newAssetId = new AssetId(registration.PluginId, registration.PartialAssetId);

        // Assigns the asset to the dict
        if (!_assetsById.TryAdd(newAssetId, registration)) {
            logger.Warning(
                "Asset with ID: {AssetId} already exists with type {ExistingAssetType}. Cannot assign a new asset with the same ID.",
                newAssetId, _assetsById[newAssetId].Type.FullName
            );
            return false;
        }
        if (!_assetsByType.TryAdd(registration.Type, newAssetId)) {
            logger.Warning(
            "Asset with ID: {AssetId} Cannot assign a new asset because it's {Type} is already assigned to another asset.",
            newAssetId, registration.Type.FullName
            );
            return false;
        }
        
        if (registration.InterfaceType is not null && !_assetsByType.TryAdd(registration.InterfaceType, newAssetId)) {
            logger.Warning(
            "Asset with ID: {AssetId} Cannot assign a new asset because it's interface {interface} is already assigned to another asset.",
            newAssetId, registration.InterfaceType.FullName
            );
            return false;
        }

        // Assign to the instance type
        if (!_assetServiceLifetimeMap.TryAddToBagOrCreateBag(registration.ServiceLifetime, newAssetId)) {
            // This shouldn't happen
            logger.Error("Asset {AssetId} could not be be mapped to an InstanceType", newAssetId);
        }

        // After Everything is said and done with the assigning, start assigning the Core tags and string tags
        List<string> tagsList = [];

        foreach (CoreTags tag in Enum.GetValuesAsUnderlyingType<CoreTags>()) {
            if (!registration.CoreTags.HasFlag(tag)) continue;
            _coreTaggedAssets.TryAddToBagOrCreateBag(tag, newAssetId);
            tagsList.Add(tag.ToString());
        }

        foreach (string stringTag in registration.StringTags) {
            if (!_stringTaggedAssets.TryAddToBagOrCreateBag(stringTag, newAssetId)) {
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
            .SelectMany(tag => _coreTaggedAssets[tag])
            .ToArray();

    public IEnumerable<AssetId> GetAllAssetsOfStringTag(string stringTag) =>
        _stringTaggedAssets.TryGetValue(stringTag, out ConcurrentBag<AssetId>? bag) ? bag : [];

    public IEnumerable<AssetId> GetAllAssetsOfPlugin(PluginId pluginId) =>
        _assetsById.Where(pair => pair.Key.PluginId == pluginId).Select(pair => pair.Key);
    
    public bool TryGetRegistration(AssetId assetId, out AssetRegistration registration) {
        return _assetsById.TryGetValue(assetId, out registration);
    }

    public bool TryGetType(AssetId assetId, [NotNullWhen(true)] out Type? type) {
        type = default;
        if (!_assetsById.TryGetValue(assetId, out AssetRegistration registration)) {
            return false;
        }

        type = registration.Type;
        return true;
    }
    public bool TryGetInterfaceType(AssetId assetId, out Type? type) {
        type = default;
        if (!_assetsById.TryGetValue(assetId, out AssetRegistration registration)) {
            return false;
        }

        type = registration.InterfaceType;
        return true;
    }

    public bool TryGetAssetId<T>(out AssetId assetId) => TryGetAssetId(typeof(T), out assetId);
    public bool TryGetAssetId(Type type, out AssetId assetId) {
        return _assetsByType.TryGetValue(type, out assetId);
    }
}