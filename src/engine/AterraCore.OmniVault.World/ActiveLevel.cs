﻿// ---------------------------------------------------------------------------------------------------------------------
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
using AterraCore.DI;
using Raylib_cs;

namespace AterraCore.OmniVault.World;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public struct ActiveLevel(INexitiesLevel2D level) : IActiveLevel {
    public INexitiesLevel2D RawLevelData { get;} = level;
    public IReadOnlyCollection<INexitiesSystem> LogicSystems { get; } = GetNexitiesSystems(level.NexitiesSystemIds.LogicSystemIds).AsReadOnly();
    public IReadOnlyCollection<INexitiesSystem> RenderSystems { get; } = GetNexitiesSystems(level.NexitiesSystemIds.RenderSystemIds).AsReadOnly();
    public IEntityNodeTree ActiveEntityTree { get; } = GetActiveEntityTree(level.InstanceId);
    
    private Ulid? _camera2D;
    public Camera2D Camera {
        get {
            Ulid id = _camera2D ??= ((ICamera2D)ActiveEntityTree.GetAsFlat().First(asset => asset is ICamera2D)).RaylibCamera2D.InstanceId;
            EngineServices.GetService<IAssetInstanceAtlas>().TryGet(id, out IRaylibCamera2D? camera2D);
            return camera2D?.Camera ?? new Camera2D();
         }
    }

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    private static INexitiesSystem[] GetNexitiesSystems(IReadOnlyCollection<AssetId> systemIds) {
        var instanceAtlas = EngineServices.GetService<IAssetInstanceAtlas>();
        
        var systems = new List<INexitiesSystem>(systemIds.Count);
        foreach (AssetId assetId in systemIds) {
            if (instanceAtlas.TryGetOrCreate(assetId, null, out INexitiesSystem? instance)) {
                systems.Add(instance);
            }
        }
        
        systems.TrimExcess();
        return systems.ToArray();
    }

    private static IEntityNodeTree GetActiveEntityTree(Ulid rootInstanceId) {
        var entityTreeFactory = EngineServices.GetService<IEntityTreeFactory>();
        return entityTreeFactory.CreateFromRootId(rootInstanceId);
    }
}
