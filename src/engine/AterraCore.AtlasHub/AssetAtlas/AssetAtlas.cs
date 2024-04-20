// // ---------------------------------------------------------------------------------------------------------------------
// // Imports
// // ---------------------------------------------------------------------------------------------------------------------
//
// using System.Collections.Concurrent;
// using System.Diagnostics.CodeAnalysis;
// using AterraCore.Contracts.AtlasHub;
// using AterraCore.Contracts.Nexities;
// using AterraCore.Types;
//
// namespace AterraCore.AtlasHub.AssetAtlas;
//
// // ---------------------------------------------------------------------------------------------------------------------
// // Code
// // ---------------------------------------------------------------------------------------------------------------------
// public class AssetAtlas : IAssetAtlas {
//     // yes we are using records here because these should just be read from, and not altered after asset atlas has been defined
//     private ConcurrentDictionary<AssetId, AssetRecord> _singletons = new();
//     private ConcurrentDictionary<AssetId, AssetRecord> _multiples = new();
//     private ConcurrentDictionary<AssetId, AssetRecord> _pooled = new();
//
//     private HashSet<AssetId>? _singletonsSetBacking;
//     internal HashSet<AssetId> SingletonsSet => _singletonsSetBacking ??= [.._singletons.Keys];
//     
//     private HashSet<AssetId>? _multiplesSetBacking;
//     internal HashSet<AssetId> MultiplesSet => _multiplesSetBacking ??= [.._multiples.Keys];
//     
//     private HashSet<AssetId>? _pooledSetBacking;
//     internal HashSet<AssetId> PooledSet => _pooledSetBacking ??= [.._pooled.Keys];
//     
//     public IReadOnlyDictionary<AssetId, AssetRecord> Singletons => _singletons.AsReadOnly();
//     public IReadOnlyDictionary<AssetId, AssetRecord> Multiples => _multiples.AsReadOnly();
//     public IReadOnlyDictionary<AssetId, AssetRecord> Pooled => _pooled.AsReadOnly();
//
//     private readonly ConcurrentBag<Type> _types = []; // Normally this shouldn't really be needed, but you never know.
//     
//     // ---------------------------------------------------------------------------------------------------------------------
//     // Methods
//     // ---------------------------------------------------------------------------------------------------------------------
//     public AssetRecord? this[AssetId id] {
//         get {
//             if (SingletonsSet.Contains(id)) return _singletons[id];
//             if (MultiplesSet.Contains(id)) return _multiples[id];
//             if (PooledSet.Contains(id)) return _pooled[id];
//             // TODO ADD LOGGING
//             return null;
//         }
//     }
//
//     public IReadOnlyDictionary<AssetId, AssetRecord> Dictionary { get; }
//
//     public bool TryRegisterAsset<T>(PluginId pluginId, PartialAssetId partialAssetId, [NotNullWhen(true)] out AssetId? registeredId) {
//         registeredId = null;
//         Type type = typeof(T);
//
//         if (_types.Contains(type)) {
//             AssetId knownId = _dictionary.FirstOrDefault(v => v.Value == type).Key;
//             throw new ArgumentException($"Type {type.Name} is already registered in the asset atlas with the AssetId of {knownId}.");
//         }
//
//         registeredId = new AssetId(pluginId, partialAssetId);
//         if (!_dictionary.TryAdd((AssetId)registeredId, type)) {
//             return false; 
//         }
//
//         _types.Add(type);
//         return true;
//     }
//
//     public bool TryGetAssetRecord(AssetId assetId, out AssetRecord? type) {
//         throw new NotImplementedException();
//     }
//
//     public bool TryGetAssetType(string assetId, out AssetRecord? type) {
//         throw new NotImplementedException();
//     }
//
//     public bool TryGetAssetRecord(AssetId assetId, [NotNullWhen(true)] out Type? type) {
//         return _dictionary.TryGetValue(assetId, out type);
//     }
//     
//     public bool TryGetAssetType(string assetId, [NotNullWhen(true)] out Type? type) {
//         return TryGetAssetRecord(new AssetId(assetId), out type);
//     }
//     
// }