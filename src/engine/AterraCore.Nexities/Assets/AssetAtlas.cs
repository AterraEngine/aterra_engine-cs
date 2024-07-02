// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Common.Types.Nexities;
using AterraCore.Contracts.Nexities.Data.Assets;
using CodeOfChaos.Extensions;
using JetBrains.Annotations;
using Serilog;
using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;

namespace AterraCore.Nexities.Assets;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------

[UsedImplicitly]
public class AssetAtlas(ILogger logger) : IAssetAtlas {
    private readonly ConcurrentDictionary<AssetId, AssetRegistration> _assetsById = new();
    private readonly ConcurrentDictionary<Type, AssetId> _assetsByType = new();
    private readonly ConcurrentDictionary<ServiceLifetimeType, ConcurrentBag<AssetId>> _assetServiceLifetimeMap = new ConcurrentDictionary<ServiceLifetimeType, ConcurrentBag<AssetId>>().PopulateWithEmpties();

    private readonly ConcurrentDictionary<CoreTags, ConcurrentBag<AssetId>> _coreTaggedAssets = new ConcurrentDictionary<CoreTags, ConcurrentBag<AssetId>>().PopulateWithEmpties();
    private readonly ConcurrentDictionary<string, ConcurrentBag<AssetId>> _stringTaggedAssets = new();

    public int TotalCount => _assetsById.Count;

    // ------------------------------------------------------------------------------------------------------------- ----
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public bool TryAssignAsset(AssetRegistration registration, [NotNullWhen(true)] out AssetId? assetId) {
        assetId = null;

        // Assigns the asset to the dict
        if (!_assetsById.TryAdd(registration.AssetId, registration)) {
            logger.Warning(
            "Asset with ID: {AssetId} already exists with type {ExistingAssetType}. Cannot assign a new asset with the same ID.",
            registration.AssetId, _assetsById[registration.AssetId].Type.FullName
            );
            return false;
        }
        if (!_assetsByType.TryAdd(registration.Type, registration.AssetId)) {
            logger.Warning(
            "Asset with ID: {AssetId} Cannot assign a new asset because it's {Type} is already assigned to another asset.",
            registration.AssetId, registration.Type.FullName
            );
            return false;
        }

        if (registration.InterfaceType is not null && !_assetsByType.TryAdd(registration.InterfaceType, registration.AssetId)) {
            logger.Warning(
            "Asset with ID: {AssetId} Cannot assign a new asset because it's interface {interface} is already assigned to another asset.",
            registration.AssetId, registration.InterfaceType.FullName
            );
            return false;
        }

        // Assign to the instance type
        if (!_assetServiceLifetimeMap.TryAddToBagOrCreateBag(registration.ServiceLifetime, registration.AssetId)) {
            // This shouldn't happen
            logger.Error("Asset {AssetId} could not be be mapped to an InstanceType", registration.AssetId);
        }

        // After Everything is said and done with the assigning, start assigning the Core tags and string tags
        List<string> tagsList = [];

        foreach (CoreTags tag in Enum.GetValuesAsUnderlyingType<CoreTags>()) {
            if (!registration.CoreTags.HasFlag(tag)) continue;
            _coreTaggedAssets.TryAddToBagOrCreateBag(tag, registration.AssetId);
            tagsList.Add(tag.ToString());
        }

        foreach (string stringTag in registration.StringTags) {
            if (!_stringTaggedAssets.TryAddToBagOrCreateBag(stringTag, registration.AssetId)) {
                logger.Warning("String Tag of {tag} could not be assigned to {assetId}", stringTag, registration.AssetId);
                continue;
            }
            tagsList.Add(stringTag);
        }

        logger.Information(
        "Assigned asset {AssetId} of Type {AssetTypeName} to plugin {PluginId} as a '{@tags}",
        registration.AssetId, registration.Type.FullName, registration.Type, tagsList
        );

        assetId = registration.AssetId;
        return true;
    }

    public IEnumerable<AssetId> GetAllAssetsOfCoreTag(CoreTags coreTag) =>
        Enum.GetValues<CoreTags>()
            .Where(tag => coreTag.HasFlag(tag))
            .SelectMany(tag => _coreTaggedAssets[tag])
            .ToArray();

    public IEnumerable<AssetId> GetAllAssetsOfStringTag(string stringTag) =>
        _stringTaggedAssets.TryGetValue(stringTag, out ConcurrentBag<AssetId>? bag) ? bag : [];

    public IEnumerable<AssetId> GetAllAssetsOfPlugin(string pluginId) => _assetsById
        .Where(pair => pair.Key.PluginId == pluginId)
        .Select(pair => pair.Key);

    public bool TryGetRegistration(AssetId assetId, out AssetRegistration registration) => _assetsById.TryGetValue(assetId, out registration);

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
    public bool TryGetAssetId(Type type, out AssetId assetId) => _assetsByType.TryGetValue(type, out assetId);
}
