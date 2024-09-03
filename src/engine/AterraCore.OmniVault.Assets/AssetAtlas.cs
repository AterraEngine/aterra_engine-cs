// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Common.Data;
using AterraCore.Common.Types.Nexities;
using AterraCore.Contracts.FlexiPlug;
using AterraCore.Contracts.OmniVault.Assets;
using AterraCore.DI;
using CodeOfChaos.Extensions;
using Extensions;
using JetBrains.Annotations;
using Serilog;
using System.Collections.Frozen;
using System.Diagnostics.CodeAnalysis;

namespace AterraCore.OmniVault.Assets;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[UsedImplicitly]
public class AssetAtlas(IPluginAtlas pluginAtlas) : IAssetAtlas {
    private readonly FrozenDictionary<AssetId, AssetRegistration> _assetsById = AssembleAssetsById(pluginAtlas);
    private readonly FrozenDictionary<Type, AssetId> _assetsByType = AssembleAssetsByType(pluginAtlas);
    private readonly FrozenDictionary<CoreTags, FrozenSet<AssetId>> _coreTaggedAssets  = AssembleCoreTaggedAssets(pluginAtlas);
    private readonly FrozenDictionary<string, FrozenSet<AssetId>> _stringTaggedAssets = AssembleStringTaggedAssets(pluginAtlas);

    public int TotalCount => _assetsById.Count;

    // -----------------------------------------------------------------------------------------------------------------
    // Constructor Methods
    // -----------------------------------------------------------------------------------------------------------------
    #region Special constructor methods
    private static FrozenDictionary<AssetId, AssetRegistration> AssembleAssetsById(IPluginAtlas pluginAtlas) {
        Dictionary<AssetId, AssetRegistration> assetsById = new();
        ILogger logger = EngineServices.GetLogger();

        foreach (AssetRegistration registration in pluginAtlas.GetAssetRegistrations()) {
            if (!assetsById.TryAdd(registration.AssetId, registration)) {
                logger.Warning(
                    "Asset with ID: {AssetId} already exists with type {ExistingAssetType}. Cannot assign a new asset with the same ID.",
                    registration.AssetId, assetsById[registration.AssetId].Type.FullName
                );
                continue;
            }
            // Assign overloads
            foreach (AssetId overridableAssetId in registration.OverridableAssetIds) {
                assetsById.AddOrUpdate(overridableAssetId, registration);

                logger.Debug(
                    "Assigned asset {AssetId} to overwrite {overridableAssetId}",
                    registration.AssetId,
                    overridableAssetId
                );
            }
        }
        return assetsById.ToFrozenDictionary();
    }
    private static FrozenDictionary<Type, AssetId> AssembleAssetsByType(IPluginAtlas pluginAtlas) {
        Dictionary<Type, AssetId> assetsByType = new();
        ILogger logger = EngineServices.GetLogger();

        foreach (AssetRegistration registration in pluginAtlas.GetAssetRegistrations()) {
            if (!assetsByType.TryAdd(registration.Type, registration.AssetId)) {
                // The reason for this, is the class type is hard linked to an AssetId
                logger.Warning(
                    "Asset with ID: {AssetId} Cannot assign a new asset because it's {Type} is already assigned to another asset.",
                    registration.AssetId, registration.Type.FullName
                );
                continue;
            }

            foreach (Type interfaceType in registration.InterfaceTypes) {
                // The reason for this, is the class type is softly linked to an AssetId, and can be overwritten
                assetsByType.AddOrUpdate(interfaceType, registration.AssetId);
                logger.Information("Asset {AssetId} linked to the interface of {Type}", registration.AssetId, interfaceType.FullName);
            }
        }
        return assetsByType.ToFrozenDictionary();
    }
    private static FrozenDictionary<CoreTags, FrozenSet<AssetId>> AssembleCoreTaggedAssets(IPluginAtlas pluginAtlas) {
        Dictionary<CoreTags, HashSet<AssetId>> coreTaggedAssets = new Dictionary<CoreTags, HashSet<AssetId>>()
            .PopulateWithEmpties<Dictionary<CoreTags, HashSet<AssetId>>, CoreTags, HashSet<AssetId>>();

        foreach (AssetRegistration registration in pluginAtlas.GetAssetRegistrations()) {
            // After Everything is said and done with the assigning, start assigning the Core tags and string tags
            foreach (CoreTags tag in Enum.GetValuesAsUnderlyingType<CoreTags>()) {
                if (!registration.CoreTags.HasFlag(tag)) continue;
                coreTaggedAssets.TryAdd(tag, []);
                coreTaggedAssets[tag].Add(registration.AssetId);
            }
        }
        return coreTaggedAssets.ToFrozenDictionary(pair => pair.Key, pair => pair.Value.ToFrozenSet());
    }
    private static FrozenDictionary<string, FrozenSet<AssetId>> AssembleStringTaggedAssets(IPluginAtlas pluginAtlas) {
        Dictionary<string, HashSet<AssetId>> stringTaggedAssets = new();

        foreach (AssetRegistration registration in pluginAtlas.GetAssetRegistrations()) {
            foreach (string stringTag in registration.StringTags) {
                stringTaggedAssets.TryAdd(stringTag, []);
                stringTaggedAssets[stringTag].Add(registration.AssetId);
            }
        }
        return stringTaggedAssets.ToFrozenDictionary(pair => pair.Key, pair => pair.Value.ToFrozenSet());
    }
    #endregion

    // ------------------------------------------------------------------------------------------------------------- ----
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public IEnumerable<AssetId> GetAllAssetsOfCoreTag(CoreTags coreTag) =>
        Enum.GetValues<CoreTags>()
            .Where(tag => coreTag.HasFlag(tag))
            .SelectMany(tag => _coreTaggedAssets[tag])
            .ToArray();

    public IEnumerable<AssetId> GetAllAssetsOfStringTag(string stringTag) =>
        _stringTaggedAssets.TryGetValue(stringTag, out FrozenSet<AssetId>? bag) ? bag : [];

    public IEnumerable<AssetId> GetAllAssetsOfPlugin(string pluginId) => _assetsById
        .Where(pair => pair.Key.PluginId == pluginId)
        .Select(pair => pair.Key);

    public Type GetAssetType(AssetId assetId) => _assetsById[assetId].Type;

    public bool TryGetRegistration(AssetId assetId, out AssetRegistration registration) => _assetsById.TryGetValue(assetId, out registration);

    public bool TryGetAssetType(AssetId assetId, [NotNullWhen(true)] out Type? type) {
        type = default;
        if (!_assetsById.TryGetValue(assetId, out AssetRegistration registration)) {
            return false;
        }

        type = registration.Type;
        return true;
    }
    public bool TryGetInterfaceTypes(AssetId assetId, out IEnumerable<Type> type) {
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
