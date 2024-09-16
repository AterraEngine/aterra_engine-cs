// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Common.Attributes.Nexities;
using AterraCore.Contracts.Nexities.Systems;
using AterraCore.Contracts.OmniVault.World;
using AterraCore.Contracts.Threading.CrossThread;
using AterraLib.Nexities.Systems.CrossThreadDataHolders;

namespace AterraLib.Nexities.Systems.Rendering;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[System(StringAssetIdLib.AterraLib.SystemsRendering.Render2D, CoreTags.RenderThread)]
[UsedImplicitly]
public class Render2D(ICrossThreadTickData crossThreadTickData) : NexitiesSystem, IRender2DSystem {
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public override void InvalidateCaches() {
        base.InvalidateCaches();

        if (!crossThreadTickData.TryGet(AssetIdLib.AterraLib.TickDataHolders.RenderableData, out RenderableData? renderableDataDto)) return;

        renderableDataDto.ClearCache();// necessary to get the correct textures later on
    }

    public void Render2DTick(IActiveLevel level) {
        if (!crossThreadTickData.TryGet(AssetIdLib.AterraLib.TickDataHolders.RenderableData, out RenderableData? renderableDataDto)) return;

        foreach (RenderCacheDto dto in renderableDataDto.GetOrderedRenderCache().AsSpan()) {
            Raylib.DrawTexturePro(dto.Texture, dto.Source, dto.Dest, dto.Origin, dto.Rotation, dto.Tint);
        }

    }
}
