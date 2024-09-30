// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.AssetVault;
using AterraCore.Contracts.Threading.CrossThread;
using Extensions;
using System.Collections.Concurrent;

namespace AterraLib.Nexities.Systems.CrossThreadDataHolders;
// ---------------------------------------------------------------------------------------------------------------------
// Support Code
// ---------------------------------------------------------------------------------------------------------------------
public readonly struct RenderCacheDto {
    public Texture2D Texture { get; init; }
    public Rectangle Source { get; init; }
    public Rectangle Dest { get; init; }
    public Vector2 Origin { get; init; }
    public float Rotation { get; init; }
    public Color Tint { get; init; }
}

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class RenderableData : AssetInstance, ITickDataHolder {
    private readonly ConcurrentSortedDictionary<int, RenderCacheDto> _renderCache = new();
    public ConcurrentDictionary<AssetId, (Vector2 Size, Texture2D texture2D)> TextureCache { get; } = new();

    public bool PropsProcessed { get; set; }

    public bool IsEmpty => true;

    public void ClearOnLevelChange() => ClearCache();
    public void ClearOnLogicTick() {}
    public void ClearOnRenderFrame() {}
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    // Method to add render cache items ensuring order
    public void AddToRenderCache(int key, RenderCacheDto dto) => _renderCache[key] = dto;
    
    public RenderCacheDto[] GetOrderedRenderCache() => _renderCache.Values;

    public void ClearCache() {
        TextureCache.Clear();
        _renderCache.Clear();
        PropsProcessed = false;
    }
}
