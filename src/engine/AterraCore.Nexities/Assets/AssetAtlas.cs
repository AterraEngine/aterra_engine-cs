// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Common.Types.Nexities;
using AterraCore.Contracts.Nexities.Data.Assets;
using AterraCore.Loggers;
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
    private ILogger Logger { get; } = logger.ForAssetAtlasContext();
    
    private readonly ConcurrentDictionary<AssetId, AssetRegistration> _assetsById = new();
    private readonly ConcurrentDictionary<Type, AssetId> _assetsByType = new();

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
            Logger.Warning(
                "Asset with ID: {AssetId} already exists with type {ExistingAssetType}. Cannot assign a new asset with the same ID.",
                registration.AssetId, _assetsById[registration.AssetId].Type.FullName
            );
            return false;
        }
        if (!_assetsByType.TryAdd(registration.Type, registration.AssetId)) {
            // The reason for this, is the class type is hard linked to an AssetId
            Logger.Warning(
                "Asset with ID: {AssetId} Cannot assign a new asset because it's {Type} is already assigned to another asset.",
                registration.AssetId, registration.Type.FullName
            );
            return false;
        }

        foreach (Type interfaceType in registration.InterfaceTypes) {
            // The reason for this, is the class type is soft linked to an AssetId, and can be overwritten
            _assetsByType.AddOrUpdate(interfaceType, registration.AssetId);
            Logger.Information("Asset {AssetId} linked to the interface of {Type}", registration.AssetId, interfaceType.FullName);
        }

        // After Everything is said and done with the assigning, start assigning the Core tags and string tags
        foreach (CoreTags tag in Enum.GetValuesAsUnderlyingType<CoreTags>()) {
            if (!registration.CoreTags.HasFlag(tag)) continue;
            _coreTaggedAssets.TryAddToBagOrCreateBag(tag, registration.AssetId);
        }

        foreach (string stringTag in registration.StringTags) {
            if (!_stringTaggedAssets.TryAddToBagOrCreateBag(stringTag, registration.AssetId)) {
                Logger.Warning("String Tag of {tag} could not be assigned to {assetId}", stringTag, registration.AssetId);
            }
        }
        
        // Assign overloads
        foreach (AssetId overridableAssetId in registration.OverridableAssetIds) {
            if (!_assetsById.TryGetValue(overridableAssetId, out AssetRegistration comparisonValue )) continue;
            if (!_assetsById.TryUpdate(overridableAssetId, registration, comparisonValue)) continue;
          
            logger.Information(
                "Assigned asset {AssetId} to overwrite {overridableAssetId}",
                registration.AssetId,
                overridableAssetId
            ); 
        }
        
        Logger.Information(
            "Assigned asset {AssetId} of Type {AssetTypeName}",
            registration.AssetId, registration.Type.FullName
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
    public bool TryGetInterfaceTypes(AssetId assetId, out Type[] type) {
        type = [];
        if (!_assetsById.TryGetValue(assetId, out AssetRegistration registration)) {
            return false;
        }

        type = registration.InterfaceTypes;
        return true;
    }

    public bool TryGetAssetId<T>(out AssetId assetId) => TryGetAssetId(typeof(T), out assetId);
    public bool TryGetAssetId(Type type, out AssetId assetId) => _assetsByType.TryGetValue(type, out assetId);
}
