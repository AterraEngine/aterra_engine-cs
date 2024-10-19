// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Common.Attributes.DI;
using AterraCore.Common.Types.Nexities;
using AterraCore.Contracts.Nexities.Components;
using AterraCore.Contracts.Nexities.Entities;
using AterraCore.Contracts.Nexities.Entities.QuickHands;
using AterraCore.Contracts.Nexities.Levels;
using AterraCore.Contracts.Nexities.Systems;
using AterraCore.Contracts.OmniVault.Assets;
using AterraCore.Contracts.OmniVault.World;
using AterraCore.Contracts.OmniVault.World.EntityTree;
using JetBrains.Annotations;
using System.Collections.Concurrent;

namespace AterraEngineOLD;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
/// <summary>
/// The ActiveLevelFactory class is responsible for creating active levels based on the provided level data.
/// </summary>
[UsedImplicitly]
[Singleton<IActiveLevelFactory>]
public class ActiveLevelFactory(IAssetInstanceAtlas instanceAtlas, IEntityTreeFactory entityTreeFactory, IAssetAtlas assetAtlas) : IActiveLevelFactory {
    /// <summary> Creates an instance of ActiveLevel using the provided INexitiesLevel as input.</summary>
    /// <param name="level2D">The INexitiesLevel object representing the level.</param>
    /// <returns>An instance of ActiveLevel.</returns>
    public ActiveLevel CreateLevel2D(INexitiesLevel level2D) {
        IEntityNodeTree entityTree = entityTreeFactory.CreateFromRootId(level2D.InstanceId);
        List<IAssetInstance> entityTreeFlat = entityTree.GetAsFlat().ToList();

        IRaylibCamera2D? camera2D = null;
        if (entityTreeFlat.FirstOrDefault(asset => asset is ICamera2D) is ICamera2D possibleCamera)
            instanceAtlas.TryGet(possibleCamera.RaylibCamera2D.InstanceId, out camera2D);

        IRenderSystem[] renderSystems = GetNexitiesSystems<IRenderSystem>(level2D.NexitiesSystemIds.AssetIds);
        return new ActiveLevel(entityTreeFactory, assetAtlas) {
            RawLevelData = level2D,
            LogicSystems = [..GetNexitiesSystems<ILogicSytem>(level2D.NexitiesSystemIds.AssetIds)],
            RenderSystems = [..renderSystems],
            RenderSystemsReversed = [..renderSystems.Reverse()],
            UiSystems = [..GetNexitiesSystems<IUiSystem>(level2D.NexitiesSystemIds.AssetIds)],
            ActiveEntityTree = entityTree,
            Camera2DEntity = camera2D,
            TextureAssetIds = new ConcurrentBag<AssetId>(entityTreeFlat
                .Where(asset => asset is IIsRenderable2D)
                .Select(asset => ((IIsRenderable2D)asset).Sprite2D.TextureAssetId)
                .Select(id => assetAtlas.TryGetRegistration(id, out AssetRegistration assetInstance) ? assetInstance.AssetId : id))
        };
    }

    /// <summary>
    ///     Creates an instance of the ActiveLevel class from a given INexitiesLevel.
    /// </summary>
    /// <param name="systemIds">The INexitiesLevel instance to create the ActiveLevel from.</param>
    /// <returns>An instance of the ActiveLevel with the specified properties populated.</returns>
    private T[] GetNexitiesSystems<T>(IReadOnlyCollection<AssetId> systemIds) where T : class, INexitiesSystem {
        var systems = new List<T>(systemIds.Count);
        foreach (AssetId assetId in systemIds) {
            if (instanceAtlas.TryGetOrCreate(assetId, out T? instance))
                systems.Add(instance);
        }

        systems.TrimExcess();
        return systems.ToArray();
    }
}
