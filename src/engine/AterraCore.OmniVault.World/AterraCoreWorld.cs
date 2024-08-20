﻿// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Common.Types.Nexities;
using AterraCore.Contracts.Nexities.Levels;
using AterraCore.Contracts.OmniVault.Assets;
using AterraCore.Contracts.OmniVault.World;
using Extensions;
using JetBrains.Annotations;
using Raylib_cs;
using Serilog;
using System.Diagnostics.CodeAnalysis;

namespace AterraCore.OmniVault.World;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[UsedImplicitly]
public class AterraCoreWorld(IAssetInstanceAtlas instanceAtlas, ILogger logger, IActiveLevelFactory levelFactory) : IAterraCoreWorld {
    private IActiveLevel? ActiveLevel { get; set; }
    
    private readonly ReaderWriterLockSlim  _activeLevelLock = new();
    private ILogger Logger { get; } = logger.ForContext<AterraCoreWorld>();

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public bool TryChangeActiveLevel(AssetId levelId) {
        // Retrieve the old level, to make comparison with, in case of loading new assets
        TryGetActiveLevel(out IActiveLevel? oldLevel);
        
        using (_activeLevelLock.Write()) {
            if (ActiveLevel?.RawLevelData.AssetId == levelId) return false;
            if(!instanceAtlas.TryGetOrCreateSingleton(levelId, out INexitiesLevel2D? level)) {
                Logger.Warning("Failed to get or create level with ID: {LevelId}", levelId);
                return false;
            }
            
            ActiveLevel = levelFactory.CreateLevel2D(level);
            
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
