// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Common.Attributes.Nexities;
using AterraCore.Common.Data;
using AterraCore.Common.Types.Nexities;
using AterraCore.Contracts.Nexities.Components;
using AterraCore.Contracts.Nexities.Entities;
using AterraCore.Contracts.Nexities.Levels;
using AterraCore.Contracts.OmniVault.Assets;
using AterraCore.Nexities.Entities;
using JetBrains.Annotations;
using Raylib_cs;
using System.Numerics;
using Workfloor_AterraCore.Plugin.Entities;

namespace Workfloor_AterraCore.Plugin.Levels;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[UsedImplicitly]
[Level(WorkfloorIdLib.Levels.Game)]
public class GameLevel(
    IDirectChildren children,
    ISystemIds systemIds,
    
    IAssetInstanceAtlas instanceAtlas
    
) : NexitiesEntity(children, systemIds), INexitiesLevel {
    
    private IDirectChildren? _children = children;
    public IDirectChildren ChildrenIDs => _children ??= GetComponent<IDirectChildren>();
    private ISystemIds? _systemIds = systemIds;
    public ISystemIds NexitiesSystemIds => _systemIds ??= GetComponent<ISystemIds>();


    public override void OnCreate(Ulid instanceId, AssetId assetId) {
        base.OnCreate(instanceId, assetId);
        
        NexitiesSystemIds.AddRange([
            AssetIdLib.AterraLib.SystemsLogic.PlayerController,
            AssetIdLib.AterraLib.SystemsLogic.CameraController,
            WorkfloorIdLib.SystemsLogic.SpawnEnemy,
            // WorkfloorIdLib.SystemsLogic.LevelSwitch,
            
            AssetIdLib.AterraLib.SystemsLogic.ApplyImpulse,
            AssetIdLib.AterraLib.SystemsLogic.ApplyImpulseCamera,
            
            AssetIdLib.AterraLib.SystemsRendering.Render2DPrepForProps,
            AssetIdLib.AterraLib.SystemsRendering.Render2DPrepForActors,
            AssetIdLib.AterraLib.SystemsRendering.Render2D,
            AssetIdLib.AterraLib.SystemsRendering.RaylibKeyHandler,
            AssetIdLib.AterraLib.SystemsRendering.RenderUi,
        ]);
        
        if (!instanceAtlas.TryCreate(WorkfloorIdLib.Entities.DuckyPlayer, out IPlayer2D? player2D)) throw new ApplicationException("Entity could not be created");
        ChildrenIDs.TryAdd(player2D);
        
        if (!instanceAtlas.TryCreate(StringAssetIdLib.AterraLib.Entities.Camera2D, out ICamera2D? camera2D)) throw new ApplicationException("Entity could not be created");

        camera2D.RaylibCamera2D.Camera = camera2D.RaylibCamera2D.Camera with {
            Target = new Vector2(0, 0),
            Offset = new Vector2(Raylib.GetScreenWidth() / 2f, Raylib.GetScreenHeight() / 2f),
            Rotation = 0,
            Zoom = 10
        };

        ChildrenIDs.TryAddFirst(camera2D.InstanceId);
        
        if (!instanceAtlas.TryCreate(WorkfloorIdLib.Entities.EntityCollection, out EntityCollection? entityCollection)) throw new ApplicationException("Entity could not be created");
        
        entityCollection.TagComponent.Tags.Add(WorkfloorIdLib.Tags.EnemyCollection);
        ChildrenIDs.TryAdd(entityCollection);
    }
}
