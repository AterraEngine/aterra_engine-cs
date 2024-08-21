// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Common.Types.Nexities;
using AterraCore.Contracts.Nexities.Components;
using AterraCore.Contracts.Nexities.Entities;
using AterraCore.Contracts.Nexities.Levels;
using AterraCore.Contracts.Nexities.Systems;
using AterraCore.Contracts.OmniVault.Assets;
using AterraCore.Contracts.OmniVault.World;
using AterraCore.Contracts.OmniVault.World.EntityTree;
using JetBrains.Annotations;
using Raylib_cs;

namespace AterraCore.OmniVault.World;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[UsedImplicitly]
public class ActiveLevelFactory(IAssetInstanceAtlas instanceAtlas, IEntityTreeFactory entityTreeFactory) : IActiveLevelFactory {
    public IActiveLevel CreateLevel2D(INexitiesLevel2D level2D) {
        IEntityNodeTree entityTree = entityTreeFactory.CreateFromRootId(level2D.InstanceId);
        List<IAssetInstance> entityTreeFlat = entityTree.GetAsFlat().ToList();

        IRaylibCamera2D? camera2D = null;
        if (entityTreeFlat.FirstOrDefault(asset => asset is ICamera2D) is ICamera2D possibleCamera )
            instanceAtlas.TryGet(possibleCamera.RaylibCamera2D.InstanceId, out camera2D);
        
        return new ActiveLevel {
            RawLevelData = level2D,
            LogicSystems = GetNexitiesSystems(level2D.NexitiesSystemIds.LogicSystemIds),
            RenderSystems = GetNexitiesSystems(level2D.NexitiesSystemIds.RenderSystemIds),
            ActiveEntityTree = entityTree,
            Camera2DEntity = camera2D,
            TextureAssetIds = entityTreeFlat
                .Where(asset => asset is IActor2D)
                .Select(asset => ((IActor2D)asset).Sprite2D.TextureAssetId)
                .Distinct()
                .ToArray()
        };
    }
    
    private INexitiesSystem[] GetNexitiesSystems(IReadOnlyCollection<AssetId> systemIds) {
        var systems = new List<INexitiesSystem>(systemIds.Count);
        foreach (AssetId assetId in systemIds)
            if (instanceAtlas.TryGetOrCreate(assetId, null, out INexitiesSystem? instance)) 
                systems.Add(instance);
        
        systems.TrimExcess();
        return systems.ToArray();
    }
}
