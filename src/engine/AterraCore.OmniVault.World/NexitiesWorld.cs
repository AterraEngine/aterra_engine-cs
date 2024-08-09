// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Common.Types.Nexities;
using AterraCore.Contracts.Nexities.Data.Levels;
using AterraCore.Contracts.Nexities.Data.Worlds;
using AterraCore.Contracts.OmniVault.Assets;
using JetBrains.Annotations;
using System.Diagnostics.CodeAnalysis;

namespace AterraCore.OmniVault.World;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[UsedImplicitly]
public class NexitiesWorld(IAssetInstanceAtlas instanceAtlas) : INexitiesWorld {
    public AssetId ActiveLevelId { get; private set; }

    private INexitiesLevel? _activeLevel;
    private INexitiesLevel? ActiveLevel {
        get {
            _cacheLock.EnterReadLock();
            try { return _activeLevel; }
            finally { _cacheLock.ExitReadLock(); }
        }
        set {
            _cacheLock.EnterWriteLock();
            try { _activeLevel = value; }
            finally{ _cacheLock.ExitWriteLock(); }
        }
    }

    private readonly ReaderWriterLockSlim _cacheLock = new();

    public bool TryChangeActiveLevel(AssetId levelId) {
        if (ActiveLevel?.AssetId == levelId) return false;
        if(!instanceAtlas.TryGetOrCreateSingleton(levelId, out INexitiesLevel? level)) return false;
        ActiveLevel = level;
        ActiveLevelId = levelId;
        return true;
    }
    
    public bool TryGetActiveLevel([NotNullWhen(true)] out INexitiesLevel? level) {
        level = ActiveLevel;
        return level != null;
    }
}
