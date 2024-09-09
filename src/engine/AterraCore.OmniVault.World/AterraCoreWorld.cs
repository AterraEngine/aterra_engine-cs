// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Attributes;
using AterraCore.Common.Types.Nexities;
using AterraCore.Contracts.Nexities.Levels;
using AterraCore.Contracts.OmniVault.Assets;
using AterraCore.Contracts.OmniVault.World;
using AterraCore.Contracts.Threading.CrossThread;
using AterraCore.Contracts.Threading.CrossThread.Dto;
using CodeOfChaos.Extensions.Serilog;
using Extensions;
using JetBrains.Annotations;
using Serilog;
using System.Diagnostics.CodeAnalysis;

namespace AterraCore.OmniVault.World;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
/// <summary>
/// Represents the core world of the Aterra system.
/// This class is responsible for managing the active level and changing levels in the game.
/// </summary>
[UsedImplicitly]
[Singleton<IAterraCoreWorld>]
public class AterraCoreWorld(
    IAssetInstanceAtlas instanceAtlas,
    ILogger logger,
    IActiveLevelFactory levelFactory,
    ICrossThreadQueue crossThreadQueue,
    ICrossThreadTickData crossThreadTickData
) : IAterraCoreWorld {
    /// <summary>
    /// Represents a Logger used for logging information and messages.
    /// </summary>
    private ILogger Logger { get; } = logger.ForContext<AterraCoreWorld>();

    /// <summary>
    /// Represents the lock object used to synchronize access to the active level in the AterraCoreWorld class.
    /// </summary>
    private readonly ReaderWriterLockSlim _activeLevelLock = new();

    /// <summary>
    /// Represents the current active level.
    /// </summary>
    public ActiveLevel? ActiveLevel { get; private set; }

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    /// <summary>
    /// Emits the active level in the AterraCoreWorld.
    /// This method is called when the active level is changed and is responsible for handling the texture asset changes.
    /// </summary>
    /// <param name="activeLevel">The new active level.</param>
    /// <param name="oldLevel">The previous active level.</param>
    private void EmitActiveLevel(ActiveLevel? activeLevel, ActiveLevel? oldLevel) {
        IEnumerable<AssetId> oldTextureAssetIds = oldLevel?.TextureAssetIds.ToArray() ?? [];
        IEnumerable<AssetId> newTextureAssetIds = activeLevel?.TextureAssetIds.ToArray() ?? [];

        Parallel.ForEach(oldTextureAssetIds.Except(newTextureAssetIds), body: id => {
            crossThreadQueue.TextureRegistrarQueue.Enqueue(new TextureRegistrar(id, true));
        });
        Parallel.ForEach(newTextureAssetIds.Except(oldTextureAssetIds), body: id => {
            crossThreadQueue.TextureRegistrarQueue.Enqueue(new TextureRegistrar(id, false));
        });
    }

    /// <summary>  Tries to change the active level in the AterraCoreWorld. </summary>
    /// <param name="levelInstance">The instance of a level to set the active level to.</param>
    /// <returns> Returns true if the active level was successfully changed, false otherwise.</returns>
    public bool TryChangeActiveLevel(INexitiesLevel levelInstance) => TryChangeActiveLevel(levelInstance.AssetId, levelInstance.InstanceId);

    /// <summary> Tries to change the active level in the AterraCoreWorld.  </summary>
    /// <param name="levelId">The ID of the level to change to.</param>
    /// <param name="levelInstanceId">The instance ID of the level to change to.</param>
    /// <returns>True if the active level was successfully changed, fal se otherwise.</returns>
    public bool TryChangeActiveLevel(AssetId levelId, Ulid? levelInstanceId = null) {
        // Retrieve the old level, to make comparison with, in case of loading new assets
        Logger.Information("Attempting to change level to {LevelId}, Instance {InstanceId}", levelId, levelInstanceId);
        TryGetActiveLevel(out ActiveLevel? oldLevel);
        using (_activeLevelLock.Write()) {
            if (ActiveLevel is { RawLevelData.InstanceId : var knownInstanceId } && knownInstanceId == levelInstanceId) {
                Logger.Warning("Can't change to the same level instance: {LevelId}", levelInstanceId);
                return false;
            }

            INexitiesLevel? level = instanceAtlas.OfAssetId<INexitiesLevel>(levelId).FirstOrDefault();

            if (level == null && !instanceAtlas.TryGetOrCreateSingleton(
                    levelId,
                    out level,
                    afterCreation: nexitiesLevel => nexitiesLevel.OnLevelFirstCreation(),
                    levelInstanceId
                )) {
                Logger.Warning("Failed to get level by instance ULID: {LevelId}", levelInstanceId);
                return false;
            }

            Logger.Information("Successfully fetched or created level. Creating ActiveLevel instance now.");
            ActiveLevel = levelFactory.CreateLevel2D(level);
            crossThreadTickData.Clear();
        }

        EmitActiveLevel(ActiveLevel, oldLevel);
        Logger.Information("Active Level successfully created.");
        return true;
    }

    /// <summary> Attempts to retrieve the current active level </summary>
    /// <param name="level">An out parameter to store the active level, if successful.</param>
    /// <returns>True if the active level is successfully retrieved; otherwise, false.</returns>
    public bool TryGetActiveLevel([NotNullWhen(true)] out ActiveLevel? level) {
        using (_activeLevelLock.Read()) {
            level = ActiveLevel;
            return level != null;
        }
    }
}
