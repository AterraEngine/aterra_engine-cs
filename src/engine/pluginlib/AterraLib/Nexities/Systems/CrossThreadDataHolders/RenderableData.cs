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
    private readonly Stack<int> _actorZIndexes = new();
    private RenderCacheTuple[] _preAllocatedCacheArray = [];
    private RenderCacheTuple[] _preAllocatedCacheArrayReverse = [];
    private bool _cacheUpdated;

    public bool PropsProcessed { get; set; }
    
    public bool IsEmpty => true;
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    // Method to add render cache items ensuring order
    private void AddToRenderCache(int key, RenderCacheTuple value) {
        _renderCache[key] = value;
        _cacheUpdated = true;
    }

    public ReadOnlySpan<RenderCacheTuple> GetOrderedRenderCache() {
        if (!_cacheUpdated) return new ReadOnlySpan<RenderCacheTuple>(_preAllocatedCacheArray, 0, _preAllocatedCacheArray.Length);

        // Use ArrayPool to minimize allocations
        if (_preAllocatedCacheArray.Length < _renderCache.Count) {
            ArrayPool<RenderCacheTuple>.Shared.Return(_preAllocatedCacheArray, clearArray: true);
            _preAllocatedCacheArray = ArrayPool<RenderCacheTuple>.Shared.Rent(_renderCache.Count);
        }
        _renderCache.Values.CopyTo(_preAllocatedCacheArray, 0);
        _cacheUpdated = false;
        return new ReadOnlySpan<RenderCacheTuple>(_preAllocatedCacheArray, 0, _renderCache.Count);
    }
    
    public ReadOnlySpan<RenderCacheTuple> GetOrderedRenderCacheReversed() {
        if (!_cacheUpdated) return new ReadOnlySpan<RenderCacheTuple>(_preAllocatedCacheArrayReverse, 0, _preAllocatedCacheArrayReverse.Length);

        // Use ArrayPool to minimize allocations
        if (_preAllocatedCacheArrayReverse.Length != _renderCache.Count) {
            ArrayPool<RenderCacheTuple>.Shared.Return(_preAllocatedCacheArrayReverse, clearArray: true);
            _preAllocatedCacheArrayReverse = ArrayPool<RenderCacheTuple>.Shared.Rent(_renderCache.Count);
        }

        // Copy elements in reverse order
        int index = 0;
        foreach (RenderCacheTuple value in _renderCache.Values) {
            _preAllocatedCacheArrayReverse[_renderCache.Count - 1 - index] = value;
            index++;
        }
  
        _cacheUpdated = false;
        return new ReadOnlySpan<RenderCacheTuple>(_preAllocatedCacheArrayReverse, 0, _renderCache.Count);
    }

    public void WritePropToRenderCache(RenderCacheTuple tuple, int zIndex) {
        AddToRenderCache(zIndex, tuple);
    }

    public void WriteActorToRenderCache(RenderCacheTuple tuple, int zIndex) {
        AddToRenderCache(zIndex, tuple);
        _actorZIndexes.Push(zIndex);
    }

    public void EndOfFrame() {
        while (_actorZIndexes.TryPop(out int zIndex)) {
            _renderCache.Remove(zIndex);
            _cacheUpdated = true;
        }
    }

    public void ClearCache() {
        TextureCache.Clear();
        _renderCache.Clear();
        ArrayPool<RenderCacheTuple>.Shared.Return(_preAllocatedCacheArray, clearArray: true);
        _preAllocatedCacheArray = ArrayPool<RenderCacheTuple>.Shared.Rent(0);
        ArrayPool<RenderCacheTuple>.Shared.Return(_preAllocatedCacheArrayReverse, clearArray: true);
        _preAllocatedCacheArrayReverse = ArrayPool<RenderCacheTuple>.Shared.Rent(0);
        _actorZIndexes.Clear();
        PropsProcessed = false;
        _cacheUpdated = true;
    }
    
    public void Clear() {}
    
}
