// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Common.Attributes.Nexities;
using AterraCore.Contracts.OmniVault.World;
using AterraCore.Contracts.Threading.CrossData;
using AterraLib.Nexities.CrossThreadDataHolders;

namespace AterraLib.Nexities.Systems.Rendering;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[System(StringAssetIdLib.AterraLib.SystemsRendering.Render2D, CoreTags.RenderThread)]
[UsedImplicitly]
public class Render2D(ICrossThreadDataAtlas crossThreadDataAtlas) : NexitiesSystem {
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public override void InvalidateCaches() {
        base.InvalidateCaches();

        if (!crossThreadDataAtlas.TryGetOrCreate(AssetIdLib.AterraLib.CrossThreadDataHolders.RenderableData, out RenderableData? renderableDataDto)) return;

        renderableDataDto.ClearCache();// necessary to get the correct textures later on
    }

    public override void Tick(ActiveLevel level) {
        if (!crossThreadDataAtlas.TryGetOrCreate(AssetIdLib.AterraLib.CrossThreadDataHolders.RenderableData, out RenderableData? renderableDataDto)) return;

        foreach (RenderCacheDto dto in renderableDataDto.GetOrderedRenderCache()) {
            Raylib.DrawTexturePro(dto.Texture, dto.Source, dto.Dest, dto.Origin, dto.Rotation, dto.Tint);
        }
        
        crossThreadDataAtlas.DataCollector.Fps = Raylib.GetFPS();
    }
}
