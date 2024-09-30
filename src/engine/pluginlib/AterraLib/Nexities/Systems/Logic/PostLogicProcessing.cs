// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Common.Attributes.Nexities;
using AterraCore.Contracts.OmniVault.World;
using AterraCore.Contracts.Threading.CrossData;
using AterraCore.Contracts.Threading.Logic;

namespace AterraLib.Nexities.Systems.Logic;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[System(StringAssetIdLib.AterraLib.SystemsLogic.PostLogicProcessing, CoreTags.LogicThread)]
[UsedImplicitly]
public class PostLogicProcessing(ILogicThreadProcessor logicThreadProcessor, ICrossThreadDataAtlas crossThreadDataAtlas) : NexitiesSystem {
    public override void Tick(ActiveLevel level) {
        crossThreadDataAtlas.DataCollector.Tps = logicThreadProcessor.TPS;
    }
}
