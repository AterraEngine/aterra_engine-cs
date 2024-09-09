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
[System(AssetIdStringLib.AterraLib.SystemsRendering.Render2DEndOfFrame, CoreTags.RenderThread)]
[UsedImplicitly]
public class Render2DEndOfFrame(ICrossThreadTickData crossThreadTickData) : NexitiesSystem {
    public override void InvalidateCaches() {
        base.InvalidateCaches();
        
        if (!crossThreadTickData.TryGet(AssetTagLib.AterraLib.RenderableData, out RenderableData? renderableDataDto)) return;
        renderableDataDto.ClearCache(); // necessary to get the correct textures later on
    }
    
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public override void Tick(ActiveLevel level) {
        if (!crossThreadTickData.TryGet(AssetTagLib.AterraLib.RenderableData, out RenderableData? renderableDataDto)) return;
        renderableDataDto.EndOfFrame();
    }
}
