// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Attributes;
using AterraCore.Common.Data;
using AterraCore.Common.Types.Nexities;
using AterraCore.Contracts.Nexities.Systems;
using AterraCore.Contracts.OmniVault.Assets;
using AterraCore.Contracts.OmniVault.World;
using AterraCore.Contracts.Threading.CrossThread;
using AterraCore.OmniVault.Assets;
using AterraLib.Contracts;
using CodeOfChaos.Extensions;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using Raylib_cs;
using Serilog;

namespace Workfloor_AterraCore.Plugin.Systems.Logic;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[System(WorkfloorIdLib.SystemsLogic.LevelSwitch, CoreTags.LogicThread)]
[Injectable<LevelSwitch>(ServiceLifetime.Singleton)]
[UsedImplicitly]
public class LevelSwitch(IAterraCoreWorld world, IAssetAtlas assetAtlas, ILogger logger, ICrossThreadTickData crossThreadTickData) : AssetInstance, INexitiesSystem {
    private List<AssetId>? _levelsCache;
    private List<AssetId> Levels => _levelsCache ??= assetAtlas.GetAllAssetsOfCoreTag(CoreTags.Level).WhereNot(id => id == AssetIdLib.AterraLib.Entities.EmptyLevel).ToList();

    public void InvalidateCaches() {
        _levelsCache = null;
    }
    public void Tick(ActiveLevel level) {
        if (!crossThreadTickData.TryGet(AssetTagLib.AterraLib.PlayerInputTickData, out ITickDataInput? playerInputTickData)) return;

        AssetId currentLevelId = level.RawLevelData.AssetId;
        int currentLevelPos = Levels.IndexOf(currentLevelId);

        KeyboardKey[] keyMovements = playerInputTickData.KeyboardKeyDown.ToHashSet().ToArray();

        for (int i = keyMovements.Length - 1; i >= 0; i--) {
            AssetId newLevelId;

            // ReSharper disable once SwitchStatementMissingSomeEnumCasesNoDefault
            switch (keyMovements[i]) {
                case KeyboardKey.KpSubtract:
                    logger.Warning("PRESSED MINUS");
                    if (currentLevelPos == 0) return;

                    newLevelId = Levels[currentLevelPos - 1];
                    if (newLevelId == level.RawLevelData.AssetId) return;
                    world.TryChangeActiveLevel(newLevelId);
                    break;
                case KeyboardKey.KpAdd:
                    logger.Warning("PRESSED PLUS");
                    if (currentLevelPos == Levels.Count - 1) return;

                    newLevelId = Levels[currentLevelPos + 1];
                    if (newLevelId == level.RawLevelData.AssetId) return;
                    world.TryChangeActiveLevel(newLevelId);
                    break;
            }
        }
    }
}
