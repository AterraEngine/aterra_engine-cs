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
public readonly struct RenderCacheDto {
    public Texture2D Texture {get; init;}
    public Rectangle Source {get; init;}
    public Rectangle Dest {get; init;}
    public Vector2 Origin {get; init;}
    public float Rotation {get; init;}
    public Color Tint {get; init;}
}

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class RenderableData : ITickDataHolder {
    public ConcurrentDictionary<AssetId, (Vector2 Size, Texture2D texture2D)> TextureCache { get; } = new();
    private readonly ConcurrentSortedDictionary<int, RenderCacheDto> _renderCache = new();

    public bool PropsProcessed { get; set; }

    public bool IsEmpty => true;
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    // Method to add render cache items ensuring order
    public void AddToRenderCache(int key, Texture2D texture, Rectangle source, Rectangle dest, Vector2 origin, float rotation, Color tint) {
        _renderCache[key] = new RenderCacheDto {
            Texture = texture,
            Source = source,
            Dest = dest,
            Origin = origin,
            Rotation = rotation,
            Tint = tint,
        };
    }
    public RenderCacheDto[] GetOrderedRenderCache() => _renderCache.Values;

    public void ClearCache() {
        TextureCache.Clear();
        _renderCache.Clear();
        PropsProcessed = false;
    }

    public void ClearOnLevelChange() => ClearCache();
    public void ClearOnLogicTick() {}
    public void ClearOnRenderFrame() {}
    
}
