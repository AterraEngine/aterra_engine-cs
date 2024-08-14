// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Common.Types.Nexities;
using AterraCore.Contracts.Nexities.Levels;
using AterraCore.Contracts.Nexities.Systems;
using AterraCore.Contracts.OmniVault.Assets;
using AterraCore.Contracts.OmniVault.World;
using JetBrains.Annotations;
using System.Diagnostics.CodeAnalysis;

namespace AterraCore.OmniVault.World;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[UsedImplicitly]
public class AterraCoreWorld(IAssetInstanceAtlas instanceAtlas) : IAterraCoreWorld {
    public AssetId ActiveLevelId { get; private set; }
    private readonly ReaderWriterLockSlim _cacheLock = new();

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
    
    private INexitiesSystem[]? _loadedLogicSystems;
    private IReadOnlyCollection<INexitiesSystem>? _loadedLogicSystemsReadOnly;
    public IReadOnlyCollection<INexitiesSystem> LogicSystems => _loadedLogicSystemsReadOnly ??= (_loadedLogicSystems ?? []).AsReadOnly();
    
    private INexitiesSystem[]? _loadedRenderSystems;
    private IReadOnlyCollection<INexitiesSystem>? _loadedRenderSystemsReadOnly;
    public IReadOnlyCollection<INexitiesSystem> RenderSystems => _loadedRenderSystemsReadOnly ??= (_loadedRenderSystems ?? []).AsReadOnly();

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public bool TryChangeActiveLevel(AssetId levelId) {
        if (ActiveLevel?.AssetId == levelId) return false;
        if(!instanceAtlas.TryGetOrCreateSingleton(levelId, out INexitiesLevel? level)) return false;
        ActiveLevel = level;
        ActiveLevelId = levelId;

        _loadedLogicSystems = level.NexitiesSystemIds.LogicSystemIds
            .Select(
                id => instanceAtlas.TryGetOrCreateSingleton(id, out INexitiesSystem? system) ? system : null
            )
            .Where(system => system is not null)
            .Select(system => system!)
            .ToArray();
        
        _loadedRenderSystems = level.NexitiesSystemIds.RenderSystemIds
            .Select(
                id => instanceAtlas.TryGetOrCreateSingleton(id, out INexitiesSystem? system) ? system : null
            )
            .Where(system => system is not null)
            .Select(system => system!)
            .ToArray();
        
        return true;
    }
    
    public bool TryGetActiveLevel([NotNullWhen(true)] out INexitiesLevel? level) {
        level = ActiveLevel;
        return level != null;
    }
}
