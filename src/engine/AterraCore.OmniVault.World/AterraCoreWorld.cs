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
            
            IEnumerable<AssetId> oldTextureAssetIds = oldLevel?.TextureAssetIds.ToArray() ?? [];
            IEnumerable<AssetId> newTextureAssetIds = ActiveLevel.TextureAssetIds.ToArray();
            
            foreach (AssetId dequeueAssetId in oldTextureAssetIds.Except(newTextureAssetIds)) 
                crossThreadQueue.TextureRegistrarQueue.Enqueue(new TextureRegistrar(dequeueAssetId, UnRegister : true ));
            foreach (AssetId enqueueAssetId in newTextureAssetIds.Except(oldTextureAssetIds)) 
                crossThreadQueue.TextureRegistrarQueue.Enqueue(new TextureRegistrar(enqueueAssetId));
            
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
