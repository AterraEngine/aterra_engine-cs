// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Common.Data;
using AterraCore.Common.Types.Nexities;
using AterraCore.Contracts.OmniVault.Assets;
using AterraCore.Contracts.PoolCorps;
using AterraCore.DI;
using JetBrains.Annotations;
using System.Collections.Concurrent;
using System.Collections.Frozen;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;

namespace AterraCore.OmniVault.Assets;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[UsedImplicitly]
public class AssetAtlas : IAssetAtlas {
    public FrozenDictionary<AssetId, AssetRegistration> AssetsById { get; internal init; } = null!;
    public FrozenDictionary<Type, AssetId> AssetsByType { get; internal init; } = null!;
    public FrozenDictionary<CoreTags, FrozenSet<AssetId>> CoreTaggedAssets { get; internal init; } = null!;
    public FrozenDictionary<string, FrozenSet<AssetId>> StringTaggedAssets { get; internal init; } = null!;

    public int TotalCount => AssetsById.Count;

    private readonly IAssetIdPools _assetIdPools = EngineServices.GetService<IAssetIdPools>();
    private readonly ConcurrentDictionary<CoreTags, FrozenSet<AssetId>> _combinedTaggedAssets = new();
    
    // ------------------------------------------------------------------------------------------------------------- ----
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public IEnumerable<AssetId> GetAllAssetsOfCoreTag(CoreTags coreTag) {
        if (_combinedTaggedAssets.TryGetValue(coreTag, out FrozenSet<AssetId>? cached)) return cached;
        
        HashSet<AssetId> hashset = _assetIdPools.AssetIdHashSetPool.Get(); // Returned on cache clear
        CoreTags[] allTags = CoreTagsExtensions.AllCoreTagValues();
        for (int i = allTags.Length - 1; i >= 0; i--) {
            CoreTags tag = allTags[i];
            if (!coreTag.HasFlag(tag)) continue;
            
            ImmutableArray<AssetId> assets = CoreTaggedAssets[tag].Items;
            for (int j = assets.Length - 1; j >= 0; j--) {
                hashset.Add(assets[j]);
            }
        }
        
        FrozenSet<AssetId> frozenSet = hashset.ToFrozenSet();
        _combinedTaggedAssets.TryAdd(coreTag, frozenSet);
        return frozenSet;
    }

    public IEnumerable<AssetId> GetAllAssetsOfStringTag(string stringTag) =>
        StringTaggedAssets.TryGetValue(stringTag, out FrozenSet<AssetId>? bag) ? bag : [];

    public IEnumerable<AssetId> GetAllAssetsOfPlugin(string pluginId) => AssetsById
        .Where(pair => pair.Key.PluginId == pluginId)
        .Select(pair => pair.Key);

    public Type GetAssetType(AssetId assetId) => AssetsById[assetId].Type;

    public bool TryGetRegistration(AssetId assetId, out AssetRegistration registration) => AssetsById.TryGetValue(assetId, out registration);

    public bool TryGetAssetType(AssetId assetId, [NotNullWhen(true)] out Type? type) {
        type = default;
        if (!AssetsById.TryGetValue(assetId, out AssetRegistration registration)) return false;

        type = registration.Type;
        return true;
    }
    public bool TryGetInterfaceTypes(AssetId assetId, out IEnumerable<Type> type) {
        type = [];
        if (!AssetsById.TryGetValue(assetId, out AssetRegistration registration)) return false;

        type = registration.InterfaceTypes;
        return true;
    }

    public bool TryGetAssetId<T>(out AssetId assetId) => TryGetAssetId(typeof(T), out assetId);
    public bool TryGetAssetId(Type type, out AssetId assetId) => AssetsByType.TryGetValue(type, out assetId);
}
