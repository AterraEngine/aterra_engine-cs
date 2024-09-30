// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Common.Attributes.Nexities;
using AterraCore.Contracts.OmniVault.World;
using AterraCore.Contracts.Threading2.CrossData;
using AterraCore.Contracts.Threading2.CrossData.Holders;

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
        if (!crossThreadDataAtlas.TryGetOrCreateDataCollector(out IDataCollector? dataCollector)) return;
        dataCollector.Fps = Raylib.GetFPS();
    }
}
