// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Common.Types.Nexities;
using Microsoft.Extensions.ObjectPool;
using System.Collections.Concurrent;

namespace AterraCore.Contracts.PoolCorps;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IAssetIdPools {
    ObjectPool<List<AssetId>> AssetIdListPool { get; }
    ObjectPool<HashSet<AssetId>> AssetIdHashSetPool { get; }
    ObjectPool<ConcurrentStack<AssetId>> AssetIdConcurrentStackPool { get; }
}
