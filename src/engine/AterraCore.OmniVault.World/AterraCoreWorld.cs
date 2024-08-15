// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Common.Types.Nexities;
using AterraCore.Contracts.Nexities.Levels;
using AterraCore.Contracts.Nexities.Systems;
using AterraCore.Contracts.OmniVault.Assets;
using AterraCore.Contracts.OmniVault.World;
using AterraCore.Contracts.OmniVault.World.EntityTree;
using JetBrains.Annotations;
using System.Diagnostics.CodeAnalysis;

namespace AterraCore.OmniVault.World;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[UsedImplicitly]
public class AterraCoreWorld(IAssetInstanceAtlas instanceAtlas) : IAterraCoreWorld {
    private IActiveLevel? ActiveLevel { get; set; }

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public bool TryChangeActiveLevel(AssetId levelId) {
        if (ActiveLevel?.RawLevelData.AssetId == levelId) return false;
        if(!instanceAtlas.TryGetOrCreateSingleton(levelId, out INexitiesLevel? level)) return false;
        ActiveLevel = new ActiveLevel(level);
        
        return true;
    }
    
    public bool TryGetActiveLevel([NotNullWhen(true)] out IActiveLevel? level) {
        level = ActiveLevel;
        return level != null;
    }
}
