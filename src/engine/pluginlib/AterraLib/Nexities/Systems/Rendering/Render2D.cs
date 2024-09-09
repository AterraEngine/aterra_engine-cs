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
    public override void Tick(ActiveLevel level) {
        if (!crossThreadTickData.TryGetOrRegister(AssetTagLib.AterraLib.RenderableData, out RenderableData? renderableDataDto)) return;

        foreach ((Texture2D texture, Rectangle source, Rectangle dest, Vector2 origin, float rotation, Color tint) tuple in renderableDataDto.RenderCache) {
            (Texture2D texture, Rectangle source, Rectangle dest, Vector2 origin, float rotation, Color tint) = tuple;
            Raylib.DrawTexturePro(texture, source, dest, origin, rotation, tint);
        }
    }
}
