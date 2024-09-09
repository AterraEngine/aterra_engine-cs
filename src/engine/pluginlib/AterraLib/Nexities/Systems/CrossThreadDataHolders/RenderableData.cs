// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Contracts.Threading.CrossThread;
using System.Buffers;
using System.Collections.Concurrent;

namespace AterraLib.Nexities.Systems.CrossThreadDataHolders;
// ---------------------------------------------------------------------------------------------------------------------
// Support Code
// ---------------------------------------------------------------------------------------------------------------------
using RenderCacheTuple = (Texture2D texture, Rectangle source, Rectangle dest, Vector2 origin, float rotation, Color tint);

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class RenderableData : ITickDataHolder {
    public ConcurrentDictionary<AssetId, (Vector2 Size, Texture2D texture2D)> TextureCache { get; } = new();
    private readonly SortedDictionary<int, RenderCacheTuple> _renderCache = new();
    private RenderCacheTuple[] _preAllocatedCacheArray = [];
    // private bool _cacheUpdated;

    public bool PropsProcessed { get; set; }
    
    public bool IsEmpty => true;
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    // Method to add render cache items ensuring order
    public void AddToRenderCache(int key, RenderCacheTuple value) {
        _renderCache[key] = value;
        // _cacheUpdated = true;
    }

    public SortedDictionary<int, RenderCacheTuple>.ValueCollection GetOrderedRenderCache() {
        return _renderCache.Values;
    }
    //private const int Start = 0;
    //public RenderCacheTuple[] GetOrderedRenderCache() {
    //    if (!_cacheUpdated) return _preAllocatedCacheArray;
    //
    //    // Use ArrayPool to minimize allocations
    //    if (_preAllocatedCacheArray.Length < _renderCache.Count) {
    //        ArrayPool<RenderCacheTuple>.Shared.Return(_preAllocatedCacheArray, clearArray: true);
    //        _preAllocatedCacheArray = ArrayPool<RenderCacheTuple>.Shared.Rent(_renderCache.Count);
    //    }
    //    _renderCache.Values.CopyTo(_preAllocatedCacheArray, Start);
    //    _cacheUpdated = false;
    //    return _preAllocatedCacheArray;
    //}

    public void ClearCache() {
        TextureCache.Clear();
        _renderCache.Clear();
        ArrayPool<RenderCacheTuple>.Shared.Return(_preAllocatedCacheArray, clearArray: true);
        _preAllocatedCacheArray = ArrayPool<RenderCacheTuple>.Shared.Rent(0);
        PropsProcessed = false;
        // _cacheUpdated = true;
    }
    
    public void Clear() {}
    
}
