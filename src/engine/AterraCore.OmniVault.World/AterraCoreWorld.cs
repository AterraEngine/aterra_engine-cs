// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Common.Types.Nexities;
using AterraCore.Contracts.Nexities.Levels;
using AterraCore.Contracts.OmniVault.Assets;
using AterraCore.Contracts.OmniVault.World;
using AterraCore.Contracts.Threading.CTQ;
using AterraCore.Contracts.Threading.CTQ.Dto;
using Extensions;
using JetBrains.Annotations;
using Serilog;
using System.Diagnostics.CodeAnalysis;

namespace AterraCore.OmniVault.World;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[UsedImplicitly]
public class AterraCoreWorld(IAssetInstanceAtlas instanceAtlas, ILogger logger, IActiveLevelFactory levelFactory, ICrossThreadQueue crossThreadQueue) : IAterraCoreWorld {
    private ILogger Logger { get; } = logger.ForContext<AterraCoreWorld>();

    private readonly ReaderWriterLockSlim  _activeLevelLock = new();
    public ActiveLevel? ActiveLevel { get; private set; }
    
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    private void EmitActiveLevel(ActiveLevel? activeLevel, ActiveLevel? oldLevel) {
        IEnumerable<AssetId> oldTextureAssetIds = oldLevel?.TextureAssetIds.ToArray() ?? [];
        IEnumerable<AssetId> newTextureAssetIds = activeLevel?.TextureAssetIds.ToArray() ?? []; 
            
        foreach (AssetId dequeueAssetId in oldTextureAssetIds.Except(newTextureAssetIds)) 
            crossThreadQueue.TextureRegistrarQueue.Enqueue(new TextureRegistrar(dequeueAssetId, UnRegister : true ));
        foreach (AssetId enqueueAssetId in newTextureAssetIds.Except(oldTextureAssetIds)) 
            crossThreadQueue.TextureRegistrarQueue.Enqueue(new TextureRegistrar(enqueueAssetId, UnRegister: false));
    }

    public bool TryChangeActiveLevel(AssetId levelId) => TryChangeActiveLevel(levelId, Ulid.NewUlid());
    public bool TryChangeActiveLevel(INexitiesLevel2D levelInstance) => TryChangeActiveLevel(levelInstance.AssetId, levelInstance.InstanceId);
    public bool TryChangeActiveLevel(AssetId levelId, Ulid levelInstanceId) {
        // Retrieve the old level, to make comparison with, in case of loading new assets
        Logger.Information("Attempting to change level to {LevelId}, Instance {InstanceId}", levelId, levelInstanceId);
        TryGetActiveLevel(out ActiveLevel? oldLevel);
        using (_activeLevelLock.Write()) {
            if (ActiveLevel is { RawLevelData.InstanceId : var knownInstanceId } && knownInstanceId == levelInstanceId) {
                Logger.Warning("Can't change to the same level instance: {LevelId}", levelInstanceId);
                return false;
            }
            if (!instanceAtlas.TryGetOrCreate(levelId, levelInstanceId, out INexitiesLevel2D? level)) {
                Logger.Warning("Failed to get level by instance ULID: {LevelId}", levelInstanceId);
                return false;
            }
            
            Logger.Information("Successfully fetched or created level. Creating ActiveLevel instance now.");
            ActiveLevel = levelFactory.CreateLevel2D(level);
        }
        
        EmitActiveLevel(ActiveLevel, oldLevel);
        Logger.Information("Active Level successfully created.");
        return true;
    }

    public bool TryGetActiveLevel([NotNullWhen(true)] out ActiveLevel? level) {
        using (_activeLevelLock.Read()) {
            level = ActiveLevel;
            return level != null;
        }
    }
}
