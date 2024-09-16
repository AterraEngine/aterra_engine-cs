// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Common.Attributes.DI;
using AterraCore.Common.Data;
using AterraCore.Common.Types.Nexities;
using AterraCore.Contracts.FlexiPlug;
using AterraCore.Contracts.OmniVault.Assets;
using AterraCore.DI;
using CodeOfChaos.Extensions;
using Extensions;
using JetBrains.Annotations;
using Serilog;
using System.Collections.Concurrent;
using System.Collections.Frozen;

namespace AterraCore.OmniVault.Assets;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[Singleton<IAssetAtlasFactory>]
[UsedImplicitly]
public class AssetAtlasFactory(IPluginAtlas pluginAtlas) : IAssetAtlasFactory {

    private readonly ConcurrentDictionary<AssetId, AssetRegistration> _assetsById = new();
    private readonly ConcurrentDictionary<Type, AssetId> _assetsByType = new();
    private readonly ConcurrentDictionary<CoreTags, ConcurrentBag<AssetId>> _coreTaggedAssets = new ConcurrentDictionary<CoreTags, ConcurrentBag<AssetId>>().PopulateWithEmpties<ConcurrentDictionary<CoreTags, ConcurrentBag<AssetId>>, CoreTags, ConcurrentBag<AssetId>>();
    private readonly ILogger _logger = EngineServices.GetLogger();
    private readonly ConcurrentDictionary<string, ConcurrentBag<AssetId>> _stringTaggedAssets = new();

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public IAssetAtlas GetAtlas() {
        AssembleAssetsById();
        AssembleAssetsByType();
        AssembleCoreTaggedAssets();
        AssembleStringTaggedAssets();

        return new AssetAtlas {
            AssetsById = _assetsById.ToFrozenDictionary(),
            AssetsByType = _assetsByType.ToFrozenDictionary(),
            CoreTaggedAssets = _coreTaggedAssets.ToFrozenDictionary(
                keySelector: pair => pair.Key,
                elementSelector: pair => pair.Value.ToFrozenSet()
            ),
            StringTaggedAssets = _stringTaggedAssets.ToFrozenDictionary(
                keySelector: pair => pair.Key,
                elementSelector: pair => pair.Value.ToFrozenSet()
            )
        };
    }

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    private void AssembleAssetsById() {
        Parallel.ForEach(pluginAtlas.GetAssetRegistrations(), body: registration => {
            if (!_assetsById.TryAdd(registration.AssetId, registration)) {
                _logger.Warning(
                    "Asset with ID: {AssetId} already exists with type {ExistingAssetType}. Cannot assign a new asset with the same ID.",
                    registration.AssetId, _assetsById[registration.AssetId].Type.FullName
                );

                return;
            }

            // Assign overloads
            foreach (AssetId overridableAssetId in registration.OverridableAssetIds) {
                if (_assetsById.TryAdd(overridableAssetId, registration)) continue;
                if (!_assetsById.TryGetValue(overridableAssetId, out AssetRegistration oldRegistration)) continue;
                if (pluginAtlas.IsLoadedAfter(oldRegistration.AssetId.PluginId, registration.AssetId.PluginId)) continue;

                _assetsById.AddOrUpdate(overridableAssetId, registration);
                _logger.Debug(
                    "Assigned asset {AssetId} to overwrite {overridableAssetId}",
                    registration.AssetId,
                    overridableAssetId
                );
            }
        });
    }
    private void AssembleAssetsByType() {
        Parallel.ForEach(pluginAtlas.GetAssetRegistrations(), body: registration => {
            if (!_assetsByType.TryAdd(registration.Type, registration.AssetId)) {
                // The reason for this, is the class type is hard linked to an AssetId
                _logger.Warning(
                    "Asset with ID: {AssetId} Cannot assign a new asset because it's {Type} is already assigned to another asset.",
                    registration.AssetId, registration.Type.FullName
                );

                return;
            }

            foreach (Type interfaceType in registration.InterfaceTypes) {
                if (!_assetsByType.TryGetValue(interfaceType, out AssetId oldAssetId)) {
                    _assetsByType.TryAdd(interfaceType, registration.AssetId);
                    _logger.Information("Asset {AssetId} linked to the interface of {Type}", registration.AssetId, interfaceType.FullName);
                    continue;
                }

                if (!_assetsById.TryGetValue(oldAssetId, out AssetRegistration oldRegistration)) continue;
                if (pluginAtlas.IsLoadedAfter(oldRegistration.AssetId.PluginId, registration.AssetId.PluginId)) continue;

                _assetsByType.TryUpdate(interfaceType, registration.AssetId, oldAssetId);
            }
        });
    }
    private void AssembleCoreTaggedAssets() {
        Parallel.ForEach(pluginAtlas.GetAssetRegistrations(), body: registration => {
            // After Everything is said and done with the assigning, start assigning the Core tags and string tags
            foreach (CoreTags tag in Enum.GetValuesAsUnderlyingType<CoreTags>()) {
                if (!registration.CoreTags.HasFlag(tag)) continue;
                if (_coreTaggedAssets.TryAdd(tag, [registration.AssetId])) continue;
                if (!_coreTaggedAssets.TryGetValue(tag, out ConcurrentBag<AssetId>? assets)) continue;

                assets.Add(registration.AssetId);
            }
        });
    }
    private void AssembleStringTaggedAssets() {
        Parallel.ForEach(pluginAtlas.GetAssetRegistrations(), body: registration => {
            foreach (string stringTag in registration.StringTags) {
                if (_stringTaggedAssets.TryAdd(stringTag, [registration.AssetId])) continue;
                if (!_stringTaggedAssets.TryGetValue(stringTag, out ConcurrentBag<AssetId>? assets)) continue;

                assets.Add(registration.AssetId);
            }
        });
    }
}
