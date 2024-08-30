// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Attributes;
using AterraCore.Common.Data;
using AterraCore.Common.Types.Nexities;
using AterraCore.Contracts.Nexities.Levels;
using AterraCore.Contracts.Nexities.Systems;
using AterraCore.Contracts.OmniVault.Assets;
using AterraCore.Contracts.OmniVault.World;
using AterraCore.OmniVault.Assets;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using Raylib_cs;
using Serilog;

namespace Workfloor_AterraCore.Plugin.Assets.Systems.Logic;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[System("Workfloor:Systems/LevelSwitch", CoreTags.LogicSystem)]
[Injectable<LevelSwitch>(ServiceLifetime.Singleton)]
[UsedImplicitly]
public class LevelSwitch(IAterraCoreWorld world, IAssetAtlas assetAtlas, ILogger logger, IAssetInstanceAtlas instanceAtlas) : AssetInstance, INexitiesSystem {
    private List<AssetId>? _levelsCache;
    private List<AssetId> Levels => _levelsCache ??= assetAtlas.GetAllAssetsOfCoreTag(CoreTags.Level).ToList();

    public void InvalidateCaches() {
        _levelsCache = null; 
    }
    public void Tick(ActiveLevel level) {
        AssetId currentLevelId = level.RawLevelData.AssetId;
        int currentLevelPos = Levels.IndexOf(currentLevelId);
        
        if (Raylib.IsKeyDown(KeyboardKey.KpSubtract)) {
            logger.Warning("PRESSED MINUS");
            if (currentLevelPos == 0) return;
            
            AssetId newLevelId = Levels[currentLevelPos-1];
            if (newLevelId == level.RawLevelData.AssetId) return;
            world.TryChangeActiveLevel(newLevelId);
        }

        if (Raylib.IsKeyDown(KeyboardKey.KpAdd)) {
            logger.Warning("PRESSED PLUS");
            if (currentLevelPos == Levels.Count - 1) return;
            
            AssetId newLevelId = Levels[currentLevelPos+1];
            if (newLevelId == level.RawLevelData.AssetId) return;
            world.TryChangeActiveLevel(newLevelId);
        }
    }
}
    
