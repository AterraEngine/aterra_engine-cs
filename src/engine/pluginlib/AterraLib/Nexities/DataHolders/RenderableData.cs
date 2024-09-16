// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Common.Attributes.Nexities;
using AterraCore.Contracts.Threading.CrossThread;
using AterraCore.OmniVault.Assets;
using Extensions;
using System.Collections.Concurrent;

namespace AterraLib.Nexities.DataHolders;
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
[UsedImplicitly]
[DataHolder(StringAssetIdLib.AterraLib.TickDataHolders.RenderableData)]
public class RenderableData : AssetInstance, ITickDataHolder, IHasLevelChangeCleanup {
    private readonly ConcurrentSortedDictionary<int, RenderCacheDto> _renderCache = new();
    public ConcurrentDictionary<AssetId, (Vector2 Size, Texture2D texture2D)> TextureCache { get; } = new();

    public bool PropsProcessed { get; set; }

    public bool IsEmpty => true;

    public void OnLevelChangeCleanup() => ClearCache();
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
