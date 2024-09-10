// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Contracts.OmniVault.World;
using AterraCore.Contracts.Threading.CrossThread;
using AterraLib.Nexities.Systems.CrossThreadDataHolders;

namespace AterraLib.Nexities.Systems.Rendering;

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

        foreach (RenderCacheDto dto in  renderableDataDto.GetOrderedRenderCache().AsSpan()) {
            Raylib.DrawTexturePro(dto.Texture, dto.Source, dto.Dest, dto.Origin, dto.Rotation, dto.Tint);
        }
            
    }
}
