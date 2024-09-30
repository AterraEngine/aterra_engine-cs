// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Common.Attributes.Nexities;
using AterraCore.Contracts.OmniVault.World;
using AterraCore.Contracts.Threading.Logic;
using AterraCore.Contracts.Threading2.CrossData;
using AterraCore.Contracts.Threading2.CrossData.Holders;

namespace AterraLib.Nexities.Systems.Logic;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[System(StringAssetIdLib.AterraLib.SystemsLogic.PostLogicProcessing, CoreTags.LogicThread)]
[UsedImplicitly]
public class PostLogicProcessing(ILogicThreadProcessor logicThreadProcessor, ICrossThreadDataAtlas crossThreadDataAtlas) : NexitiesSystem {
    public override void Tick(ActiveLevel level) {
        if (!crossThreadDataAtlas.TryGetOrCreateDataCollector(out IDataCollector? dataCollector)) return;
        dataCollector.Tps = logicThreadProcessor.TPS;
    }
}
