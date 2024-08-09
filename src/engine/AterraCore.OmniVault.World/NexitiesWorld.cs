// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Common.Types.Nexities;
using AterraCore.Contracts.Nexities.Data.Levels;
using AterraCore.Contracts.Nexities.Data.Worlds;
using AterraCore.Contracts.OmniVault.Assets;
using JetBrains.Annotations;

namespace AterraCore.OmniVault.World;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[UsedImplicitly]
public class NexitiesWorld(IAssetInstanceAtlas instanceAtlas) : INexitiesWorld {
    public AssetId ActiveLevelId { get; private set; }
    public INexitiesLevel? ActiveLevel { get; private set; }

    public bool TryChangeActiveLevel(AssetId levelId) {
        if (ActiveLevel?.AssetId == levelId) return false;
        if(!instanceAtlas.TryGetOrCreateSingleton(levelId, out INexitiesLevel? level)) return false;
        ActiveLevel = level;
        ActiveLevelId = levelId;
        return true;
    }
}
