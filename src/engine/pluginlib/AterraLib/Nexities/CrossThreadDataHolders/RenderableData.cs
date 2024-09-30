// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.AssetVault;
using AterraCore.Common.Attributes.AssetVault;
using AterraCore.Contracts.Threading.CrossData;
using System.Collections.Concurrent;

namespace AterraLib.Nexities.CrossThreadDataHolders;
// ---------------------------------------------------------------------------------------------------------------------
// Support Code
// ---------------------------------------------------------------------------------------------------------------------
public readonly struct RenderCacheDto {
    public int ZIndex { get; init; }
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
[UsedImplicitly]
[CrossThreadDataHolder(StringAssetIdLib.AterraLib.CrossThreadDataHolders.RenderableData)]
public class RenderableData : AssetInstance, ICrossThreadData {
    private readonly SortedDictionary<int, RenderCacheDto> _renderCache = new();
    public ConcurrentDictionary<AssetId, (Vector2 Size, Texture2D texture2D)> TextureCache { get; } = new();

    public bool PropsProcessed { get; set; }

    // Method to add or replace render cache items based on ZIndex
    public void AddToRenderCache(RenderCacheDto dto) {
        _renderCache[dto.ZIndex] = dto;
    }

    // Get the ordered render cache items based on ZIndex
    public IEnumerable<RenderCacheDto> GetOrderedRenderCache() => _renderCache.Values;

    public void ClearCache() {
        TextureCache.Clear();
        _renderCache.Clear();
        PropsProcessed = false;
    }
}
