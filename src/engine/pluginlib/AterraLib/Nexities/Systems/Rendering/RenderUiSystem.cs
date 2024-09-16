// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Common.Attributes.Nexities;
using AterraCore.Contracts.Nexities.Systems;
using AterraCore.Contracts.OmniVault.Assets;
using AterraCore.Contracts.OmniVault.DataCollector;
using AterraCore.Contracts.OmniVault.World;

namespace AterraLib.Nexities.Systems.Rendering;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[System(StringAssetIdLib.AterraLib.SystemsRendering.RenderUi, CoreTags.RenderThread)]
[UsedImplicitly]
public class RenderUiSystem(IDataCollector dataCollector, IAssetInstanceAtlas instanceAtlas) : NexitiesSystem, IRenderUiSystem {
    public void RenderUITick(IActiveLevel level) {
        Raylib.DrawRectangle(0, 0, 250, 50 * 9, new Color(0, 0, 0, 127));

        Raylib.DrawText($"   FPS : {dataCollector.Fps}", 0, 0, 32, Color.LightGray);
        Raylib.DrawText($"minFPS : {dataCollector.FpsMin}", 0, 50, 32, Color.LightGray);
        Raylib.DrawText($"maxFPS : {dataCollector.FpsMax}", 0, 100, 32, Color.LightGray);
        Raylib.DrawText($"avgFPS : {dataCollector.FpsAverageString}", 0, 150, 32, Color.LightGray);
        Raylib.DrawText($"   TPS : {dataCollector.Tps}", 0, 200, 32, Color.LightGray);
        Raylib.DrawText($"avgTPS : {dataCollector.TpsAverageString}", 0, 250, 32, Color.LightGray);
        Raylib.DrawText($" DUCKS : {level.RawLevelData.ChildrenIDs.Count}", 0, 300, 32, Color.LightGray);
        Raylib.DrawText($"entGlb : {instanceAtlas.TotalCount}", 0, 350, 32, Color.LightGray);
        Raylib.DrawText($" Asset : {level.RawLevelData.AssetId}", 0, 400, 12, Color.LightGray);
        Raylib.DrawText($"  Inst : {level.RawLevelData.InstanceId}", 0, 425, 12, Color.LightGray);
    }
}
