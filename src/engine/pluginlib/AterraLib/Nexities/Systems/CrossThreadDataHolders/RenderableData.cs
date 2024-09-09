// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Contracts.Threading.CrossThread;
using System.Collections.Immutable;

namespace AterraLib.Nexities.Systems.CrossThreadDataHolders;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class RenderableData : ITickDataHolder {
    public ImmutableDictionary<AssetId, (Vector2 Size, Texture2D texture2D)> TextureCache { get; set; } = ImmutableDictionary<AssetId, (Vector2 Size, Texture2D texture2D)>.Empty;
    public List<( Texture2D texture, Rectangle source, Rectangle dest, Vector2 origin, float rotation, Color tint)> RenderCache { get; set; } = [];
    public Dictionary<Ulid, int> InstanceToIndexInRenderCache { get; set; } = new();
    public bool PropsProcessed = false;
    
    public bool IsEmpty => false;
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public void Clear() {
        
    }
}
