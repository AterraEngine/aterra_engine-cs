// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Common.Types.Nexities;
using AterraCore.Contracts.Nexities.Levels;
using AterraCore.Contracts.OmniVault.Assets;
using AterraCore.Contracts.OmniVault.World;
using Extensions;
using JetBrains.Annotations;
using Serilog;
using System.Diagnostics.CodeAnalysis;

namespace AterraCore.OmniVault.World;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[UsedImplicitly]
public class AterraCoreWorld(IAssetInstanceAtlas instanceAtlas, ILogger logger) : IAterraCoreWorld {
    private IActiveLevel? ActiveLevel { get; set; }
    private readonly ReaderWriterLockSlim  _activeLevelLock = new();
    private ILogger Logger { get; } = logger.ForContext<AterraCoreWorld>();

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public bool TryChangeActiveLevel(AssetId levelId) {
        using (_activeLevelLock.Write()) {
            if (ActiveLevel?.RawLevelData.AssetId == levelId) return false;
            if(!instanceAtlas.TryGetOrCreateSingleton(levelId, out INexitiesLevel2D? level)) {
                Logger.Warning("Failed to get or create level with ID: {LevelId}", levelId);
                return false;
            }
            ActiveLevel = new ActiveLevel(level);
            return true;
        }
    }
    
    public bool TryGetActiveLevel([NotNullWhen(true)] out IActiveLevel? level) {
        using (_activeLevelLock.Read()) {
            level = ActiveLevel;
            return level != null;
        }
    }
}
