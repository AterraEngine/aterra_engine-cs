// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Contracts.Threading.CrossThread;
using Extensions;
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
    private readonly ConcurrentSortedDictionary<int, RenderCacheTuple> _renderCache = new();

    public bool PropsProcessed { get; set; }

    public bool IsEmpty => true;
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    // Method to add render cache items ensuring order
    public void AddToRenderCache(int key, RenderCacheTuple value) => _renderCache[key] = value;
    public RenderCacheTuple[] GetOrderedRenderCache() => _renderCache.Values;

    public void ClearCache() {
        TextureCache.Clear();
        _renderCache.Clear();
        PropsProcessed = false;
    }

    public void ClearOnLevelChange() => ClearCache();
    public void ClearOnLogicTick() {}
    public void ClearOnRenderFrame() {}
    
}
