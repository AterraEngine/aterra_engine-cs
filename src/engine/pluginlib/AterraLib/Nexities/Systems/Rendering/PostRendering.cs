// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Common.Attributes.Nexities;
using AterraCore.Contracts.OmniVault.World;
using AterraCore.Contracts.Threading.CrossData;

namespace AterraLib.Nexities.Systems.Rendering;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[System(StringAssetIdLib.AterraLib.SystemsRendering.PostRendering, CoreTags.RenderThread)]
[UsedImplicitly]
public class PostRendering(ICrossThreadDataAtlas crossThreadDataAtlas) : NexitiesSystem {
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public override void Tick(ActiveLevel level) {
        crossThreadDataAtlas.DataCollector.Fps = Raylib.GetFPS();
    }
}
