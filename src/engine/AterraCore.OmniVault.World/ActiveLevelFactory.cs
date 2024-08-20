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

namespace AterraCore.OmniVault.World;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[UsedImplicitly]
public class ActiveLevelFactory(IAssetInstanceAtlas instanceAtlas, IEntityTreeFactory entityTreeFactory) : IActiveLevelFactory {
    public IActiveLevel CreateLevel2D(INexitiesLevel2D level2D) {
        IEntityNodeTree entityTree = entityTreeFactory.CreateFromRootId(level2D.InstanceId);
        instanceAtlas.TryGet(
            ((ICamera2D)entityTree.GetAsFlat().First(asset => asset is ICamera2D)).RaylibCamera2D.InstanceId,
            out IRaylibCamera2D? camera2D
        );
        
        return new ActiveLevel {
            RawLevelData = level2D,
            LogicSystems = GetNexitiesSystems(level2D.NexitiesSystemIds.LogicSystemIds),
            RenderSystems = GetNexitiesSystems(level2D.NexitiesSystemIds.RenderSystemIds),
            ActiveEntityTree = entityTree,
            Camera2DEntity = camera2D!
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
