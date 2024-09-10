// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Contracts.OmniVault.World;
using AterraCore.Contracts.Threading.CrossThread;
using AterraLib.Nexities.Systems.CrossThreadDataHolders;

namespace AterraLib.Nexities.Systems.Rendering;

// ---------------------------------------------------------------------------------------------------------------------
// Support Code
// ---------------------------------------------------------------------------------------------------------------------
using RenderCacheTuple = (Texture2D texture, Rectangle source, Rectangle dest, Vector2 origin, float rotation, Color tint);

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[System(AssetIdStringLib.AterraLib.SystemsRendering.Render2D, CoreTags.RenderThread)]
[UsedImplicitly]
public class Render2D(ICrossThreadTickData crossThreadTickData) : NexitiesSystem {
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public override void InvalidateCaches() {
        base.InvalidateCaches();
        
        if (!crossThreadTickData.TryGet(AssetTagLib.AterraLib.RenderableData, out RenderableData? renderableDataDto)) return;
        renderableDataDto.ClearCache(); // necessary to get the correct textures later on
    }
    
    public override void Tick(ActiveLevel level) {
        if (!crossThreadTickData.TryGet(AssetTagLib.AterraLib.RenderableData, out RenderableData? renderableDataDto)) return;

        IEnumerable<RenderCacheTuple> valueCollection = renderableDataDto.GetOrderedRenderCache();
        foreach (RenderCacheTuple tuple in valueCollection) {
            Raylib.DrawTexturePro(tuple.texture, tuple.source, tuple.dest, tuple.origin, tuple.rotation, tuple.tint);
            
        }
            
    }
}
